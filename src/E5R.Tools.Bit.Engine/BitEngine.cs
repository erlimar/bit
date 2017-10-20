// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace E5R.Tools.Bit.Engine
{
    using Sdk.Bit.Command;
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
            var engine = new BitEngine(container);

            container.AddSingleton<BitEngine>(engine);

            return engine;
        }
    }
}
