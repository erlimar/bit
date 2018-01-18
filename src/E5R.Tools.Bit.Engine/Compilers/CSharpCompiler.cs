// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.Extensions.Logging;

using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace E5R.Tools.Bit.Engine.Compilers
{
    using Sdk.Bit.Command;

    public class CSharpCompiler : IBitCompiler
    {
        private readonly ILogger _logger;

        public CSharpCompiler(ILogger<CSharpCompiler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public byte[] CompileByteCode(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create(null)
                .WithOptions(options)
                .AddReferences(GetSystemReferences())
                .AddSyntaxTrees(tree);

            EmitResult result = null;
            byte[] bytesResult = null;

            using (var ms = new MemoryStream())
            {
                result = compilation.Emit(ms);

                if (result.Success)
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    bytesResult = ms.ToArray();
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

            return bytesResult;
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
    }
}
