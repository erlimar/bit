// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

const string PRODUCT_NAME = "bit";
const string PRODUCT_VERSION = "1.0.0";

var outputDirectory = MakeAbsolute(Directory("./dist/"));
var outputAppRootDirectory = MakeAbsolute(Directory("./dist/app"));
var outputAppDirectory = MakeAbsolute(Directory("./dist/app/{runtime}"));
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");
var versionSuffix = Argument("version-suffix", "");
var productVersion = PRODUCT_VERSION + (!string.IsNullOrEmpty(versionSuffix)
    ? string.Format("-{0}", versionSuffix)
    : string.Empty);

Task("Clean")
    .Does(() =>
{
    var settings = new DeleteDirectorySettings
    {
        Recursive = true,
        Force = true
    };

    var directories = Enumerable.Empty<DirectoryPath>()
        .Concat(GetDirectories(outputDirectory.FullPath))
        .Concat(GetDirectories(string.Format("./src/**/bin/{0}", configuration)))
        .Concat(GetDirectories(string.Format("./src/**/obj/{0}", configuration)))
        .Concat(GetDirectories(string.Format("./test/**/bin/{0}", configuration)))
        .Concat(GetDirectories(string.Format("./test/**/obj/{0}", configuration)));

    var files = Enumerable.Empty<FilePath>()
        .Concat(GetFiles("./src/**/obj/*.nuspec"));

    DeleteDirectories(directories, settings);
    DeleteFiles(files);
});

Task("FullClean")
    .Does(() =>
{
    var settings = new DeleteDirectorySettings
    {
        Recursive = true,
        Force = true
    };

    var directories = Enumerable.Empty<DirectoryPath>()
        .Concat(GetDirectories(outputDirectory.FullPath))
        .Concat(GetDirectories("./src/**/bin"))
        .Concat(GetDirectories("./src/**/obj"))
        .Concat(GetDirectories("./test/**/bin"))
        .Concat(GetDirectories("./test/**/obj"));

    DeleteDirectories(directories, settings);
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var settings = new DotNetCoreRestoreSettings
    {
        NoCache = true
    };

    DotNetCoreRestore(settings);
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings
    {
        Configuration = configuration
    };

    DotNetCoreBuild("E5R.Bit.sln", settings);
});

Task("Test")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var settings = new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild = false
    };

    var projects = GetFiles("./test/**/*.Test.csproj");

    foreach(var project in projects)
    {
        DotNetCoreTest(project.FullPath, settings);
    }
});

Task("Pack-Sdk")
    .IsDependentOn("Build")
    .Does(() =>
{
    var settings = new DotNetCorePackSettings
    {
        Configuration = configuration,
        IncludeSource = false,
        NoBuild = true,
        IncludeSymbols = configuration.ToLower() == "debug",
        OutputDirectory = outputDirectory
    };

    if(!string.IsNullOrEmpty(versionSuffix))
    {
        settings.VersionSuffix = versionSuffix;
    }

    EnsureDirectoryExists(outputDirectory);

    var projects = GetFiles("./src/E5R.Sdk.*/E5R.Sdk.*.csproj");

    foreach(var project in projects)
    {
        DotNetCorePack(project.FullPath, settings);
    }
});

Task("Publish-App")
    .Does(() =>
{
    var project = MakeAbsolute(File("./src/E5R.Tools.Bit/E5R.Tools.Bit.csproj"));
    var runtimes = XmlPeek(project.FullPath, "/Project/PropertyGroup/RuntimeIdentifiers");
    var args = string.Format("--self-contained --configuration \"{0}\"", configuration);
    var settings = new DeleteDirectorySettings
    {
        Recursive = true,
        Force = true
    };

    if(!string.IsNullOrEmpty(versionSuffix))
    {
        args += string.Format(" --version-suffix \"{0}\"", versionSuffix);
    }

    args += " --output \"{output}\"";
    args += " --runtime \"{runtime}\"";

    if(DirectoryExists(outputAppRootDirectory.FullPath))
    {
        DeleteDirectory(outputAppRootDirectory, settings);
    }

    foreach(var runtime in runtimes.Split(';'))
    {
        var toolArgs = args;
        var outputPath = outputAppDirectory.FullPath.Replace("{runtime}", runtime);

        toolArgs = toolArgs.Replace("{output}", outputPath);
        toolArgs = toolArgs.Replace("{runtime}", runtime);
        
        Information(string.Format("Pack {0} app...", runtime));
        EnsureDirectoryExists(outputPath);
        DotNetCoreTool(project.FullPath, "publish", toolArgs);
    }
});

Task("Pack-App")
    .IsDependentOn("Publish-App")
    .Does(() =>
{
    var directories = Enumerable.Empty<DirectoryPath>()
        .Concat(GetDirectories(System.IO.Path.Combine(outputAppRootDirectory.FullPath, "*")));

    foreach(var dir in directories)
    {
        var zipFile = string.Format("{0}-{1}-{2}.zip", 
            PRODUCT_NAME, productVersion,
            System.IO.Path.GetFileName(dir.FullPath));
        var zipPath = File(string.Format("{0}/../{1}", System.IO.Path.GetDirectoryName(dir.FullPath), zipFile));

        Zip(dir, zipPath);
    }
});

Task("Pack")
    .IsDependentOn("Pack-Sdk")
    .IsDependentOn("Pack-App");

Task("Default")
    .IsDependentOn("Test");

RunTarget(target);
