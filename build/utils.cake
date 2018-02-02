// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

public class BuildUtils
{
    private readonly ICakeContext _ctx;
    private readonly BuildOptions _options;

    public BuildUtils(ICakeContext ctx, BuildOptions options)
    {
        _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public IEnumerable<DirectoryPath> GetDirectoriesToClean()
    {
        return Enumerable.Empty<DirectoryPath>()
            .Concat(_ctx.GetDirectories(_options.OutputDirectory.FullPath))
            .Concat(_ctx.GetDirectories(string.Format("./src/**/bin/{0}", _options.Configuration)))
            .Concat(_ctx.GetDirectories(string.Format("./src/**/obj/{0}", _options.Configuration)))
            .Concat(_ctx.GetDirectories(string.Format("./test/**/bin/{0}", _options.Configuration)))
            .Concat(_ctx.GetDirectories(string.Format("./test/**/obj/{0}", _options.Configuration)));
    }

    public IEnumerable<DirectoryPath> GetDirectoriesToFullClean()
    {
        return Enumerable.Empty<DirectoryPath>()
            .Concat(_ctx.GetDirectories(options.OutputDirectory.FullPath))
            .Concat(_ctx.GetDirectories("./src/**/bin"))
            .Concat(_ctx.GetDirectories("./src/**/obj"))
            .Concat(_ctx.GetDirectories("./test/**/bin"))
            .Concat(_ctx.GetDirectories("./test/**/obj"));
    }

    public IEnumerable<FilePath> GetFilesToClean()
    {
        return Enumerable.Empty<FilePath>()
            .Concat(_ctx.GetFiles("./src/**/obj/*.nuspec"));
    }
}