// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;

namespace E5R.Tools.Bit.Engine.DI
{
    using Sdk.Bit.Services.Abstractions;
    using Engine.Services;
    using Engine.Compilers;

    public class BitEngineContainer : DependencyInjectionContainer
    {
        public BitEngineContainer()
            : base(new ServiceCollection(), DefaultServicesFactory, LoggingConfigure)
        { }

        private static IEnumerable<KeyValuePair<Type, Type>> DefaultServicesFactory()
        {
            yield return NewTypeKeyValuePair<BitConfiguration, IBitConfiguration>();
            yield return NewTypeKeyValuePair<BitDiscovery, IBitDiscovery>();
            yield return NewTypeKeyValuePair<BitEnvironment, IBitEnvironment>();
            yield return NewTypeKeyValuePair<CSharpCompiler, IBitCompiler>();
        }

        private static KeyValuePair<Type, Type> NewTypeKeyValuePair<T1, T2>()
        {
            return new KeyValuePair<Type, Type>(typeof(T1), typeof(T2));
        }

        private static void LoggingConfigure(ILoggingBuilder builder) => builder.AddConsole();
        protected override IServiceProvider GetProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
