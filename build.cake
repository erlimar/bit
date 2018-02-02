// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

// Load scripts
#load "./build/options.cake"
#load "./build/utils.cake"
#load "./build/boost.cake"

var options = new BuildOptions(Context);
var utils = new BuildUtils(Context, options);

Task("Clean")
    .Does(() =>
{
    var settings = new DeleteDirectorySettings
    {
        Recursive = true,
        Force = true
    };

    DeleteDirectories(utils.GetDirectoriesToClean(), settings);
    DeleteFiles(utils.GetFilesToClean());
});

Task("FullClean")
    .Does(() =>
{
    var settings = new DeleteDirectorySettings
    {
        Recursive = true,
        Force = true
    };

    DeleteDirectories(utils.GetDirectoriesToFullClean(), settings);
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
        Configuration = options.Configuration
    };

    DotNetCoreBuild("E5R.Bit.sln", settings);
});

Task("Test")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var settings = new DotNetCoreTestSettings
    {
        Configuration = options.Configuration,
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
        Configuration = options.Configuration,
        IncludeSource = false,
        NoBuild = true,
        IncludeSymbols = options.Configuration.ToLower() == "debug",
        OutputDirectory = options.OutputDirectory
    };

    if(!string.IsNullOrEmpty(options.VersionSuffix))
    {
        settings.VersionSuffix = options.VersionSuffix;
    }

    EnsureDirectoryExists(options.OutputDirectory);

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
    var args = string.Format("--self-contained --configuration \"{0}\"", options.Configuration);
    var settings = new DeleteDirectorySettings
    {
        Recursive = true,
        Force = true
    };

    if(!string.IsNullOrEmpty(options.VersionSuffix))
    {
        args += string.Format(" --version-suffix \"{0}\"", options.VersionSuffix);
    }

    args += " --output \"{output}\"";
    args += " --runtime \"{runtime}\"";

    if(DirectoryExists(options.OutputAppRootDirectory.FullPath))
    {
        DeleteDirectory(options.OutputAppRootDirectory, settings);
    }

    foreach(var runtime in runtimes.Split(';'))
    {
        var toolArgs = args;
        var outputPath = options.OutputAppDirectory.FullPath.Replace("{runtime}", runtime);

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
        .Concat(GetDirectories(System.IO.Path.Combine(options.OutputAppRootDirectory.FullPath, "*")));

    foreach(var dir in directories)
    {
        var zipFile = string.Format("{0}-{1}-{2}.zip", 
            options.ProductName, options.ProductVersion,
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

Task("Bootstrap")
    .Does(() => {
        Information("Setuping building system...");

        var boost = new BuildBoost(Context, options);
        
        Information("--> Boost...");
        boost.EnsureSources();
        Information("--> Boost.OK!");

        Information("Building system ready!");
     } );

RunTarget(options.Target);
