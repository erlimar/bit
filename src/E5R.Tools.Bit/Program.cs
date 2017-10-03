using System;

namespace E5R.Tools.Bit
{
    // http://www.tugberkugurlu.com/archive/compiling-c-sharp-code-into-memory-and-executing-it-with-roslyn
    // https://msdn.microsoft.com/pt-br/magazine/mt808499.aspx
    // https://github.com/dotnet/core/blob/master/Documentation/self-contained-linux-apps.md
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class BitCommandDescriptor {}
    public class BitHelp {}

    public class BitCommand
    {
        public Bitcommand()
        {
            CommandDescriptor = new BitCommandDescriptor();
            Help = new BitHelp(CommandDescriptor);
        }
        
        protected BitCommandDescriptor CommandDescriptor { get; }
        protected BitHelp Help { get; }
    }

    // https://gist.github.com/iamarcel/9bdc3f40d95c13f80d259b7eb2bbcabb
    // https://gist.github.com/iamarcel/8047384bfbe9941e52817cf14a79dc34
    // https://www.areilly.com/2017/04/21/command-line-argument-parsing-in-net-core-with-microsoft-extensions-commandlineutils/
    // https://msdn.microsoft.com/en-us/magazine/mt763239.aspx
    // http://fclp.github.io/fluent-command-line-parser/
    public class MyClass : BitCommand
    {
        public MyClass()
        {
            CommandDescriptor.Define(_ => {
                _.Name("MyCommand");
                _.Description("My Command sample for developer user");
                
                _.Option("version").Alias("v")
                    .Description("Version of SDK")
                    .Sample("-v latest", "Install the latest version of SDK")
                    .Sample("-version 5.0", "Install the 5.0 minor version of SDK")
                    .Sample("-v 6", "Install the 6 major version of SDK");
                    
                /**
                * @Help output:
                *
                * ```
                $ bit mycommand --help
                $ bit help mycommand
                
                My Command sample for developer user
                
                Usage: bit [global-options] mycommand [command-options]
                
                global-options:
                --verbosity|-V         Active log verbosity
                
                command-options:
                --version|-v           Version of SDK
                
                    e.g: -v latest       Install the latest version of SDK
                        -version 5.0    Install the 5.0 minor version of SDK
                        -v 6            Install the 6 major version of SDK
                * ```
                */
            });
        }
        
        public BitResult Run(BitArguments args)
        {
        }
    }
}
