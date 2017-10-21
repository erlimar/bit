// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace E5R.Tools.Bit.Engine.DI
{
    using Sdk.Bit.Services.Abstractions;
    using Engine.Services;
    using Engine.Compilers;

    public class BitEngineContainer : DependencyInjectionContainer
    {
        public BitEngineContainer() : base(new ServiceCollection()) { }

        protected override void AddDefaultServices(IServiceCollection services)
        {
            services.AddSingleton<IBitConfiguration, BitConfiguration>();
            services.AddSingleton<IBitDiscovery, BitDiscovery>();
            services.AddSingleton<IBitEnvironment, BitEnvironment>();
            services.AddSingleton<IBitCompiler, CSharpCompiler>();

            services.AddLogging(builder =>
            {
                builder.AddConsole();
            });
        }

        protected override IServiceProvider GetProvider(IServiceCollection services) => services.BuildServiceProvider();
    }
}
