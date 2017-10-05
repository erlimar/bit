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
    public class BitCommand {}

    // http://www.tugberkugurlu.com/archive/compiling-c-sharp-code-into-memory-and-executing-it-with-roslyn
    // https://msdn.microsoft.com/pt-br/magazine/mt808499.aspx
    // https://github.com/dotnet/core/blob/master/Documentation/self-contained-linux-apps.md

    // https://gist.github.com/iamarcel/9bdc3f40d95c13f80d259b7eb2bbcabb
    // https://gist.github.com/iamarcel/8047384bfbe9941e52817cf14a79dc34
    // https://www.areilly.com/2017/04/21/command-line-argument-parsing-in-net-core-with-microsoft-extensions-commandlineutils/
    // https://msdn.microsoft.com/en-us/magazine/mt763239.aspx
    // http://fclp.github.io/fluent-command-line-parser/
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Running code:");
            Console.WriteLine(CSharpCodeBlock);

            var assembly = GetAssemblyFromCodeBlock(CSharpCodeBlock);

            Console.WriteLine("BitCommand's identified:");

            foreach(var t in assembly.GetTypes().Where(t => t.IsPublic && t.IsSubclassOf(typeof(BitCommand))))
            {
                Console.WriteLine($"   * {t.FullName}");
            }
            
            return 0;
        }

        static Assembly GetAssemblyFromCodeBlock(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var compilation = CSharpCompilation.Create(null)
                .WithOptions(options)
                .AddSyntaxTrees(tree)
                .AddReferences(GetSystemReferences());

            EmitResult result = null;
            Assembly assembly = null;

            using(var ms = new MemoryStream())
            {
                result = compilation.Emit(ms);

                if(result.Success)
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    assembly = Assembly.Load(ms.ToArray());
                }
            }

            if(result != null && !result.Success)
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
            var bitAssemblyPath = Assembly.GetExecutingAssembly().Location;
            var systemPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            Func<string, MetadataReference> Ref = (s) => {
                return MetadataReference.CreateFromFile(Path.Combine(systemPath, $"{s}.dll"));
            };

            return new List<MetadataReference>()
            {
                MetadataReference.CreateFromFile(bitAssemblyPath),
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
        using E5R.Tools.Bit;

        namespace MyCompany.Components.Utils
        {
            public class Command : BitCommand
            {
                public void PrintMessage()
                {
                    int a = 1 + 2;
                    Console.WriteLine($""Hello World from Command! {a}"");
                }
            }

            class OtherClass {}
            public class PublicOtherClass {}
        }
        ";
    }
}
