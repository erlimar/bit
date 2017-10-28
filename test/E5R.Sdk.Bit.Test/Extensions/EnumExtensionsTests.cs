// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using Xunit;

namespace E5R.Sdk.Bit.Test.Extensions
{
    using Bit.Utils;
    using _ = E5R.Test.Commons.TraitConstants;

    [Trait(_.MODULE, "E5R.Sdk.Bit")]
    [Trait(_.COMPONENT, nameof(EnumExtensions))]
    [Trait(_.CATEGORY, "UNIT")]
    public class EnumExtensionsTests
    {
        [Fact(DisplayName = "GetDescription: BitUriScheme.Command == \"cmd\"")]
        public void GetDescription_BitUriScheme_Command_Is_cmd()
        {
            Assert.Equal("cmd", BitUriScheme.Command.GetDescription());
        }

        [Fact(DisplayName = "GetDescription: BitUriScheme.Library == \"lib\"")]
        public void GetDescription_BitUriScheme_Library_Is_cmd()
        {
            Assert.Equal("lib", BitUriScheme.Library.GetDescription());
        }

        [Fact(DisplayName = "GetDescription: BitUriScheme.Documentation == \"doc\"")]
        public void GetDescription_BitUriScheme_Documentation_Is_cmd()
        {
            Assert.Equal("doc", BitUriScheme.Documentation.GetDescription());
        }
    }
}
