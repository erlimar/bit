// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace E5R.Tools.Bit.Engine.DI
{
    public class DependencyInjectionContainer
    {
        private readonly IServiceCollection _services;
        private readonly Func<IEnumerable<KeyValuePair<Type, Type>>> _defaultServicesFactory;
        private readonly Action<ILoggingBuilder> _loggingConfigure;
        private bool defaultServicesLoaded = false;

        public DependencyInjectionContainer(IServiceCollection services,
            Func<IEnumerable<KeyValuePair<Type, Type>>> defaultServicesFactory, Action<ILoggingBuilder> loggingConfigure)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _defaultServicesFactory = defaultServicesFactory ?? throw new ArgumentNullException(nameof(defaultServicesFactory));
            _loggingConfigure = loggingConfigure ?? throw new ArgumentNullException(nameof(loggingConfigure));
        }

        protected virtual IServiceProvider GetProvider(IServiceCollection services) => throw new NotImplementedException();
        public static DependencyInjectionContainer BuildDefault() => new BitEngineContainer();

        private void EnsureDefaultServices()
        {
            if (defaultServicesLoaded)
            {
                return;
            }

            foreach (var pair in _defaultServicesFactory())
            {
                if (pair.Key == null)
                {
                    _services.AddSingleton(pair.Value);
                }
                else
                {
                    _services.AddSingleton(pair.Value, pair.Key);
                }
            }

            _services.AddLogging(_loggingConfigure);

            defaultServicesLoaded = true;
        }

        internal void AddSingleton<T>(T serviceType)
            where T : class
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            EnsureDefaultServices();
            _services.AddSingleton<T>(serviceType);
        }

        public IServiceProvider GetProvider()
        {
            EnsureDefaultServices();
            return GetProvider(_services);
        }

        public void Add(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            EnsureDefaultServices();
            _services.AddTransient(serviceType);
        }
    }
}
