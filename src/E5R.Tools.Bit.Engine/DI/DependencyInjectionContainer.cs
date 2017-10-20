// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace E5R.Tools.Bit.Engine.DI
{
    public class DependencyInjectionContainer
    {
        private readonly IServiceCollection _services;

        public DependencyInjectionContainer(IServiceCollection services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));

            AddDefaultServices(_services);
        }

        protected virtual void AddDefaultServices(IServiceCollection services) => throw new NotImplementedException();
        protected virtual IServiceProvider GetProvider(IServiceCollection services) => throw new NotImplementedException();

        public IServiceProvider GetProvider() => GetProvider(_services);

        public void Add(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            _services.AddTransient(serviceType);
        }

        public static DependencyInjectionContainer BuildDefault()
        {
            return new BitEngineContainer();
        }
    }
}
