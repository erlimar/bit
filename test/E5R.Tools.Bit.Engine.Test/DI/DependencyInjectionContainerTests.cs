// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using Microsoft.Extensions.DependencyInjection;

namespace E5R.Tools.Bit.Engine.Test.DI
{
    using Engine.DI;
    using Microsoft.Extensions.Logging;
    using _ = E5R.Test.Commons.TraitConstants;

    [Trait(_.MODULE, "E5R.Tools.Bit.Engine")]
    [Trait(_.COMPONENT, nameof(DependencyInjectionContainer))]
    [Trait(_.CATEGORY, "UNIT")]
    public class DependencyInjectionContainerTests
    {
        [Fact(DisplayName = "Constructor does not accept null for [services] parameter.")]
        public void Constructor_Does_Not_Accept_Null()
        {
            var servicesMock = new Mock<IServiceCollection>().Object;
            var defaultServicesFactoryMock = new Mock<Func<IEnumerable<KeyValuePair<Type, Type>>>>().Object;
            var loggingConfigureMock = new Mock<Action<ILoggingBuilder>>().Object;

            var error1 = Assert.Throws<ArgumentNullException>(() => new DependencyInjectionContainer(null, defaultServicesFactoryMock, loggingConfigureMock));
            Assert.Equal("services", error1.ParamName);

            var error2 = Assert.Throws<ArgumentNullException>(() => new DependencyInjectionContainer(servicesMock, null, loggingConfigureMock));
            Assert.Equal("defaultServicesFactory", error2.ParamName);

            var error3 = Assert.Throws<ArgumentNullException>(() => new DependencyInjectionContainer(servicesMock, defaultServicesFactoryMock, null));
            Assert.Equal("loggingConfigure", error3.ParamName);
        }

        [Fact(DisplayName = "Add method does not accept null for [serviceType] parameter.")]
        public void Add_Does_Not_Accept_Null()
        {
            var servicesMock = new Mock<IServiceCollection>().Object;
            var providerMock = new Mock<IServiceProvider>().Object;
            var containerMock = new DIContainer(servicesMock, providerMock);

            var error = Assert.Throws<ArgumentNullException>(() => containerMock.Add(null));
            Assert.Equal("serviceType", error.ParamName);
        }

        private class DIContainer : DependencyInjectionContainer
        {
            private readonly IServiceProvider _provider;

            public DIContainer(IServiceCollection services, IServiceProvider provider)
                : base(services, DefaultServicesFactory, LoggingConfigure)
            {
                _provider = provider;
            }

            public static IEnumerable<KeyValuePair<Type, Type>> DefaultServicesFactory()
            {
                return new Dictionary<Type, Type>();
            }

            public static void LoggingConfigure(ILoggingBuilder builder)
            { /* void */ }

            protected override IServiceProvider GetProvider(IServiceCollection services)
            {
                return _provider;
            }
        }
    }
}
