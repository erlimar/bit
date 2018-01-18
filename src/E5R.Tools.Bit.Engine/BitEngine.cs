// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;

namespace E5R.Tools.Bit.Engine
{
    using Engine.DI;

    public class BitEngine
    {
        private readonly DependencyInjectionContainer _container;

        public BitEngine(DependencyInjectionContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public void RegisterType(Type type) => _container.Add(type);

        public T ResolveType<T>(Type type) where T : class
            => _container.GetProvider().GetService(type) as T;

        public static BitEngine Build(DependencyInjectionContainer container)
        {
            return new BitEngine(container);
        }
    }
}
