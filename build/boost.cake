// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

public class BuildBoost
{
    private readonly ICakeContext _ctx;
    private readonly BuildOptions _options;

    private string _extractDir;
    private string _installDir;
    private string _sourceFileName;
    private string _sourceFilePath;
    private string _sourceUrl;
    private string _sourceLegacyUrl;
    private string _bootstrapFilePath;

    public BuildBoost(ICakeContext ctx, BuildOptions options)
    {
        _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        _options = options ?? throw new ArgumentNullException(nameof(options));

        MakeOptions();
    }

    public void EnsureSources()
    {
        _ctx.Verbose("SOURCE_FILE_NAME: " + _sourceFileName);
        _ctx.Verbose("SOURCE_FILE_PATH: " + _sourceFilePath);
        _ctx.Verbose("INSTALL_DIR: " + _installDir);
        _ctx.Verbose("EXTRACT_DIR: " + _extractDir);
        _ctx.Verbose("SOURCE_URL: " + _sourceUrl);
        _ctx.Verbose("SOURCE_LEGACY_URL: " + _sourceLegacyUrl);
        _ctx.Verbose("BOOTSTRAP_FILE_PATH: " + _bootstrapFilePath);

        if(!_ctx.FileExists(_bootstrapFilePath))
        {
            _ctx.Verbose("Boost not found! Acquiring...");
            DownloadSource();
            ExtractSouce();
            RemoveSourceFile();
        }
    }

    private void MakeOptions()
    {
        _extractDir = _options.BuildRootDirectory.FullPath;
        _installDir = _options.BuildRootDirectory
            .Combine(_ctx.Directory(".boost"))
            .FullPath;
        _sourceFileName = $"boost_{_options.BoostVersion.Replace('.','_')}.zip";
        _sourceFilePath = _ctx.File(string.Format("{0}/{1}", _options.BuildRootDirectory.FullPath, _sourceFileName));
        _sourceUrl = $"https://dl.bintray.com/boostorg/release/{_options.BoostVersion}/source/{_sourceFileName}";
        _sourceLegacyUrl = $"https://sourceforge.net/projects/boost/files/boost/{_options.BoostVersion}/{_sourceFileName}/download";

        if(_ctx.IsRunningOnWindows())
        {
            _bootstrapFilePath = _ctx.File(string.Format("{0}/{1}", _installDir, "bootstrap.bat"));
        } else {
            _bootstrapFilePath = _ctx.File(string.Format("{0}/{1}", _installDir, "bootstrap.sh"));
        }

    }

    private void DownloadSource()
    {
        if(_ctx.FileExists(_sourceFilePath))
        {
            _ctx.Verbose("Removing old boost source file...");
            RemoveSourceFile();
        }

        _ctx.Verbose("Trying to download boost source file...");
        try { _ctx.DownloadFile(_sourceUrl, _sourceFilePath); } catch { }

        if(_ctx.FileExists(_sourceFilePath))
        {
            return;
        }

        _ctx.Verbose("Trying to download boost source file from legacy URL...");
        try { _ctx.DownloadFile(_sourceLegacyUrl, _sourceFilePath); } catch { }

        if(!_ctx.FileExists(_sourceFilePath))
        {
            throw new Exception("Failed to download boost source file!");
        }
    }

    private void ExtractSouce()
    {
        if(!_ctx.FileExists(_sourceFilePath))
        {
            throw new Exception("Boost source file not found!");
        }

        if(_ctx.IsRunningOnWindows())
        {
            _ctx.Verbose("Extracting boost source code with PowerShell...");
            ExtractWithPowerShell();
        } else {
            _ctx.Verbose("Extracting boost source code...");
            _ctx.Unzip(_sourceFilePath, _extractDir);
        }

        // TODO: Rename _extractDir to _installDir;

        if(!_ctx.FileExists(_bootstrapFilePath))
        {
            throw new Exception("Failed to unzip boost source file!");
        }
    }

    private void ExtractWithPowerShell()
    {
        string args = "";

        args += $" -File .\\build\\unzip.ps1";
        args += $" -ZipPath \"{_sourceFilePath}\"";
        args += $" -DirectoryPath \"{_installDir}\"";

        var exitCode = _ctx.StartProcess("powershell", args);
        
        _ctx.Information("PowerShell exit code: {0}", exitCode);
    }

    private void RemoveSourceFile()
    {
        if(_ctx.FileExists(_sourceFilePath))
        {
            _ctx.DeleteFile(_sourceFilePath);
        }
    }
}