// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using Xunit;

namespace E5R.Tools.Bit.Engine.Test
{
    using _ = E5R.Test.Commons.TraitConstants;

    [Trait(_.MODULE, "E5R.Tools.Bit.Engine")]
    [Trait(_.COMPONENT, nameof(BitOutput))]
    [Trait(_.CATEGORY, "UNIT")]
    public class BitOutputTests
    {
        [Fact(DisplayName = "Must construct an object without parameters")]
        public void Must_Construct_Object_Without_Parameters()
        {
            Assert.NotNull(new BitOutput());
        }
    }
}
