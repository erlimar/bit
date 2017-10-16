// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace E5R.Tools.Bit
{
    using Sdk.Bit.Services.Abstractions;
    using Sdk.Bit.Services;

    public class BitContainer
    {
        private readonly IServiceCollection _services;

        public BitContainer()
        {
            _services = new ServiceCollection();

            AddSdkServices();
        }

        private void AddSdkServices()
        {
            _services.AddSingleton<IBitConfiguration, BitConfiguration>();
        }

        public void Add(Type serviceType)
        {
            _services.AddTransient(serviceType);
        }

        public ServiceProvider GetProvider()
        {
            return _services.BuildServiceProvider();
        }
    }
}
