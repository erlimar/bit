using System;
//using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace E5R.Tools.Bit
{
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

            ExecCSharpCodeBlock(CSharpCodeBlock);
            
            return 0;
        }

        static void ExecCSharpCodeBlock(string code)
        {
            var node = CSharpSyntaxTree.ParseText(code);

            Console.ReadLine();
        }

        static string CSharpCodeBlock = @"
        public class Command
        {
            public void PrintMessage()
            {
                Console.WriteLine(""Hello World from Command!"");
            }
        }
        ";
    }
}
