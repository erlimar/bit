// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");
var versionSuffix = Argument("version-suffix", "");

Task("Clean")
    .Does(() =>
{
    var settings = new DeleteDirectorySettings
    {
        Recursive = true,
        Force = true
    };

    var directories = Enumerable.Empty<DirectoryPath>()
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
    .IsDependentOn("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var settings = new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild = true
    };

    var projects = GetFiles("./test/**/*.Test.csproj");

    foreach(var project in projects)
    {
        DotNetCoreTest(project.FullPath, settings);
    }
});

Task("Pack")
    .IsDependentOn("Build")
    .Does(() =>
{
    var settings = new DotNetCorePackSettings
    {
        Configuration = configuration,
        IncludeSource = false,
        NoBuild = true,
        IncludeSymbols = configuration.ToLower() == "debug"
    };

    if(!string.IsNullOrEmpty(versionSuffix))
    {
        settings.VersionSuffix = versionSuffix;
    }

    var projects = GetFiles("./src/E5R.Sdk.*/E5R.Sdk.*.csproj");

    foreach(var project in projects)
    {
        DotNetCorePack(project.FullPath, settings);
    }
});

Task("Default")
    .IsDependentOn("Test");

RunTarget(target);
