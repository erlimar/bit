// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using Xunit;
using Moq;
using Microsoft.Extensions.DependencyInjection;

namespace E5R.Tools.Bit.Engine.Test
{
    using _ = E5R.Test.Commons.TraitConstants;

    [Trait(_.MODULE, "E5R.Tools.Bit.Engine")]
    [Trait(_.COMPONENT, nameof(DependencyInjectionContainer))]
    public class DependencyInjectionContainerTests
    {
        [Fact(DisplayName = "Constructor does not accept null for [services] parameter.")]
        public void Constructor_Does_Not_Accept_Null()
        {
            var error = Assert.Throws<ArgumentNullException>(() => new DependencyInjectionContainer(null));
            Assert.Equal("services", error.ParamName);
        }

        [Fact(DisplayName = "Add method does not accept null for [serviceType] parameter.")]
        public void Add_Does_Not_Accept_Null()
        {
            var servicesMock = new Mock<IServiceCollection>().Object;
            var containerMock = new MockDependencyInjectionContainer(servicesMock);

            var error = Assert.Throws<ArgumentNullException>(() => containerMock.Add(null));
            Assert.Equal("serviceType", error.ParamName);
        }
    }

    internal class MockDependencyInjectionContainer : DependencyInjectionContainer
    {
        public MockDependencyInjectionContainer(IServiceCollection services) : base(services) { }

        protected override void AddDefaultServices(IServiceCollection services)
        {
            /* void */
        }
    }
}
