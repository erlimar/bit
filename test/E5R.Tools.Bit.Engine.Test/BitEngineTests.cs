// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using Xunit;

namespace E5R.Tools.Bit.Engine.Test
{
    using _ = E5R.Test.Commons.TraitConstants;

    [Trait(_.MODULE, "E5R.Tools.Bit.Engine")]
    [Trait(_.COMPONENT, nameof(BitEngine))]
    [Trait(_.CATEGORY, "UNIT")]
    public class BitEngineTests
    {
        [Fact(DisplayName ="Parameter container is required")]
        public void Container_Is_Required()
        {
            var error = Assert.Throws<ArgumentNullException>(() => new BitEngine(null));
            Assert.Equal("container", error.ParamName);
        }
    }
}
