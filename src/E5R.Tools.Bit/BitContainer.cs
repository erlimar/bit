// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using Microsoft.Extensions.DependencyInjection;
using System;

namespace E5R.Tools.Bit
{
    using Sdk.Bit.Services;
    using Sdk.Bit.Services.Abstractions;
    using Engine;

    public class BitContainer : DependencyInjectionContainer
    {
        public BitContainer() : base(new ServiceCollection()) { }

        protected override void AddDefaultServices(IServiceCollection services)
        {
            services.AddSingleton<IBitConfiguration, BitConfiguration>();
        }

        protected override IServiceProvider GetProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }
    }
}
