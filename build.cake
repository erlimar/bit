var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

Task("Clean")
    .Does(() =>
{
    DotNetCoreClean("E5R.Bit.sln");
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore();
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    DotNetCoreBuild("E5R.Bit.sln");
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    var projects = GetFiles("./test/**/*Test.csproj");

    foreach(var project in projects)
    {
        DotNetCoreTest(project.FullPath);
    }
});

Task("Default")
    .IsDependentOn("Test");

RunTarget(target);
