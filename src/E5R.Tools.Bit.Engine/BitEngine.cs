// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace E5R.Tools.Bit.Engine
{
    using Sdk.Bit.Command;
    using Engine.DI;

    public class BitEngine
    {
        private readonly DependencyInjectionContainer _container;

        public BitEngine(DependencyInjectionContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        private IEnumerable<MetadataReference> GetSystemReferences()
        {
            var sdkAssemblyPath = typeof(BitCommand).Assembly.Location;
            var clrPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

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

            MetadataReference Ref(string s)
            {
                return MetadataReference.CreateFromFile(Path.Combine(path1: clrPath, path2: $"{s}.dll"));
            };
        }

        public Assembly CompileToAssembly(string code)
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
                    // TODO: Move to ILog
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

        public void RegisterType(Type type) => _container.Add(type);

        public T ResolveType<T>(Type type) where T : class
            => _container.GetProvider().GetService(type) as T;

        public static BitEngine Build(DependencyInjectionContainer container)
        {
            return new BitEngine(container);
        }
    }
}
