// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

public class BuildOptions
{
    // TODO: Load from /src/E5R.Tools.Bit/E5R.Tools.Bit.csproj
    //       or
    //       /src/Shared/**/*c.s
    // XmlPeek(project.FullPath, "/Project/PropertyGroup/Name|Version");
    const string PRODUCT_NAME = "bit";
    const string PRODUCT_VERSION = "1.0.0";

    private readonly ICakeContext _ctx;

    public DirectoryPath OutputDirectory { get; private set; }
    public DirectoryPath OutputAppRootDirectory { get; private set; }
    public DirectoryPath OutputAppDirectory { get; private set; }
    public string Target { get; private set; }
    public string Configuration { get; private set; }
    public string VersionSuffix { get; private set; }
    public string ProductVersion { get; private set; }
    public string ProductName { get; private set; } = PRODUCT_NAME;

    public BuildOptions(ICakeContext ctx)
    {
        _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));

        MakeOptions();
    }

    private void MakeOptions()
    {
        OutputDirectory = _ctx.MakeAbsolute(_ctx.Directory("./dist/"));
        OutputAppRootDirectory = _ctx.MakeAbsolute(_ctx.Directory("./dist/app"));
        OutputAppDirectory = _ctx.MakeAbsolute(_ctx.Directory("./dist/app/{runtime}"));
        Target = _ctx.Argument("target", "Default");
        Configuration = _ctx.Argument("configuration", "Debug");
        VersionSuffix = _ctx.Argument("version-suffix", string.Empty);
        ProductVersion = PRODUCT_VERSION + (!string.IsNullOrEmpty(VersionSuffix)
            ? string.Format("-{0}", VersionSuffix)
            : string.Empty);
    }
}