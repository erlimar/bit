// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using System.Linq;

namespace E5R.Tools.Bit
{
    using Engine;
    using Sdk.Bit.Services.Abstractions;

    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Running code:");
            Console.WriteLine(CSharpCodeBlock);

            var engine = new BitEngine(new BitContainer());
            var assembly = engine.CompileToAssembly(CSharpCodeBlock);

            if (assembly == null)
            {
                return 1;
            }

            Console.WriteLine("BitCommand's identified:");

            foreach (var t in assembly.GetTypes().Where(t => typeof(IBitCommand).IsAssignableFrom(t)))
            {
                Console.WriteLine($"   * {t.FullName}");
                engine.RegisterType(t);
                var instance = engine.ResolveType<IBitCommand>(t);
                var result = instance.Main(null);
            }

            return 0;
        }

        static string CSharpCodeBlock = @"
        using System;
        using E5R.Sdk.Bit.Command;
        using E5R.Sdk.Bit.Services.Abstractions;

        namespace MyCompany.Components.Utils
        {
            public class Command : IBitCommand
            {
                private readonly IBitConfiguration _config;

                public Command(IBitConfiguration config)
                {
                    _config = config ?? throw new ArgumentNullException(nameof(config));
                }

                public string GetEncodingName()
                {
                    return _config.DefaultEncoding.BodyName;
                }
            }

            class OtherClass {}
            public class PublicOtherClass {}
        }
        ";
    }
}
