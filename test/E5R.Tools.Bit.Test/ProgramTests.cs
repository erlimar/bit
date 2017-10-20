// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Moq;

namespace E5R.Tools.Bit.Test
{
    using Engine;
    using Engine.DI;
    using Sdk.Bit.Services.Abstractions;
    using _ = E5R.Test.Commons.TraitConstants;

    [Trait(_.MODULE, "E5R.Tool.Bit")]
    [Trait(_.COMPONENT, nameof(Program))]
    public class ProgramTests
    {
        [Fact(DisplayName = "Constructor does not accept null for [env, container] parameters.")]
        public void Constructor_Does_Not_Accept_Null()
        {
            var envMock = new Mock<IBitEnvironment>().Object;
            var servicesMock = new Mock<IServiceCollection>().Object;
            var containerMock = new Mock<DependencyInjectionContainer>(servicesMock).Object;

            var errorEnv = Assert.Throws<ArgumentNullException>(() => new Program(null, containerMock));
            Assert.Equal("env", errorEnv.ParamName);

            var errorContainer = Assert.Throws<ArgumentNullException>(() => new Program(envMock, null));
            Assert.Equal("container", errorContainer.ParamName);
        }
    }
}
