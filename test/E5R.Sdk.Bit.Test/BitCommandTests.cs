// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using Xunit;

namespace E5R.Sdk.Bit.Test
{
    using _ = E5R.Test.Utils.TraitConstants;

    [Trait(_.MODULE, "E5R.Sdk.Bit")]
    [Trait(_.COMPONENT, nameof(BitCommand))]
    public class BitCommandTests
    {
        [Fact(DisplayName = "Must instantiate without parameters")]
        public void Must_Instantiate_Without_Parameters()
        {
            // Arrange
            BitCommand instance;

            // Act
            instance = new BitCommand();

            // Assert
            Assert.NotNull(instance);
        }
    }
}
