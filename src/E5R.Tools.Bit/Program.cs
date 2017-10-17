// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace E5R.Tools.Bit
{
    using Sdk.Bit.Command;

    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Running code:");
            Console.WriteLine(CSharpCodeBlock);

            var assembly = GetAssemblyFromCodeBlock(CSharpCodeBlock);

            if (assembly == null)
            {
                return 1;
            }

            var container = new BitContainer();

            Console.WriteLine("BitCommand's identified:");

            foreach (var t in assembly.GetTypes().Where(t => typeof(IBitCommand).IsAssignableFrom(t)))
            {
                Console.WriteLine($"   * {t.FullName}");
                container.Add(t);
                var instance = container.GetProvider().GetService(t) as IBitCommand;
                Console.WriteLine($"   > Instance.EncodingName: {instance.GetEncodingName()}");
            }

            return 0;
        }

        static Assembly GetAssemblyFromCodeBlock(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create(null)
                .WithOptions(options)
                .AddReferences(GetSystemReferences())
                .AddSyntaxTrees(tree);

            EmitResult result = null;
            Assembly assembly = null;

            using (var ms = new MemoryStream())
            {
                result = compilation.Emit(ms);

                if (result.Success)
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    assembly = Assembly.Load(ms.ToArray());
                }
            }

            if (result != null && !result.Success)
            {
                IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

                foreach (var failure in failures)
                {
                    Console.WriteLine("Error on compile!");
                    Console.WriteLine($"  ID: {failure.Id}");
                    Console.WriteLine($"  Message: {failure.GetMessage()}");
                    Console.WriteLine($"  Location: {failure.Location.GetLineSpan()}");
                    Console.WriteLine($"  Severity: {failure.Severity}");
                }

                return null;
            }

            return assembly;
        }

        static IEnumerable<MetadataReference> GetSystemReferences()
        {
            var sdkAssemblyPath = typeof(IBitCommand).Assembly.Location;
            var systemPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            Func<string, MetadataReference> Ref = (s) =>
            {
                return MetadataReference.CreateFromFile(Path.Combine(systemPath, $"{s}.dll"));
            };

            return new List<MetadataReference>()
            {
                MetadataReference.CreateFromFile(sdkAssemblyPath),
                Ref("mscorlib"),
                Ref("netstandard"),
                Ref("System"),
                Ref("System.Collections"),
                Ref("System.Console"),
                Ref("System.Core"),
                Ref("System.Linq"),
                Ref("System.Private.CoreLib"),
                Ref("System.Runtime")
            };
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
