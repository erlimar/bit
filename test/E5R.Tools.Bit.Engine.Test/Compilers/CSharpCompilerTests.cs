// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using Xunit;

namespace E5R.Tools.Bit.Engine.Test.Compilers
{
    using Engine.Compilers;
    using _ = E5R.Test.Commons.TraitConstants;

    [Trait(_.MODULE, "E5R.Tools.Bit.Engine")]
    [Trait(_.COMPONENT, nameof(CSharpCompiler))]
    [Trait(_.CATEGORY, "UNIT")]
    public class CSharpCompilerTests
    {
        [Fact(DisplayName = "Constructor does not accept null for [services] parameter.")]
        public void Constructor_Does_Not_Accept_Null_Parameter()
        {
            var error = Assert.Throws<ArgumentNullException>(() => new CSharpCompiler(null));
            Assert.Equal("logger", error.ParamName);
        }
    }
}
