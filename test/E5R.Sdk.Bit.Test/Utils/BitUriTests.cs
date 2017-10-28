// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using Xunit;

namespace E5R.Sdk.Bit.Test.Utils
{
    using Bit.Utils;
    using _ = E5R.Test.Commons.TraitConstants;

    [Trait(_.MODULE, "E5R.Sdk.Bit")]
    [Trait(_.COMPONENT, nameof(BitUri))]
    [Trait(_.CATEGORY, "UNIT")]
    public class BitUriTests
    {
        [Theory(DisplayName = "ParseOrDefault: Returns null to bad Uri")]
        [InlineData("")]
        [InlineData("http://site.com")]
        [InlineData("mailto://jose@net")]
        [InlineData("This is invalid")]
        public void ParseOrDefault_Returns_Null_To_Bad_Uri(string badUri)
        {
            Assert.Null(BitUri.ParseOrDefault(badUri));
        }

        [Theory(DisplayName = "ParseOrDefault: Returns not null to good Uri")]
        [InlineData("cmd://resource@repository/version")]
        [InlineData("cmd://resource@repository")]
        [InlineData("cmd://resource/version")]
        [InlineData("cmd://resource")]
        [InlineData("lib://resource@repository/version")]
        [InlineData("lib://resource@repository")]
        [InlineData("lib://resource/version")]
        [InlineData("lib://resource")]
        [InlineData("doc://resource@repository/version")]
        [InlineData("doc://resource@repository")]
        [InlineData("doc://resource/version")]
        [InlineData("doc://resource")]
        public void ParseOrDefault_Returns_NotNull_To_Good_Uri(string goodUri)
        {
            Assert.NotNull(BitUri.ParseOrDefault(goodUri));
        }

        [Theory(DisplayName = "ParseOrDefault: Returns correct information for all schemes")]
        [InlineData("cmd")]
        [InlineData("lib")]
        [InlineData("doc")]
        public void PaseorDefault_Return_Correct_Info_For_All_Schemes(string scheme)
        {
            var uri1 = BitUri.ParseOrDefault($"{scheme}://resource@repository/version");
            var uri2 = BitUri.ParseOrDefault($"{scheme}://resource@repository");
            var uri3 = BitUri.ParseOrDefault($"{scheme}://resource/version");
            var uri4 = BitUri.ParseOrDefault($"{scheme}://resource");

            Assert.Equal(uri1.Scheme.GetDescription(), scheme);
            Assert.Equal(uri2.Scheme.GetDescription(), scheme);
            Assert.Equal(uri3.Scheme.GetDescription(), scheme);
            Assert.Equal(uri4.Scheme.GetDescription(), scheme);

            Assert.Equal("resource", uri1.Resource);
            Assert.Equal("repository", uri1.Repository);
            Assert.Equal("version", uri1.Version);

            Assert.Equal("resource", uri2.Resource);
            Assert.Equal("repository", uri2.Repository);
            Assert.Equal(string.Empty, uri2.Version);

            Assert.Equal("resource", uri3.Resource);
            Assert.Equal(string.Empty, uri3.Repository);
            Assert.Equal("version", uri3.Version);

            Assert.Equal("resource", uri4.Resource);
            Assert.Equal(string.Empty, uri4.Repository);
            Assert.Equal(string.Empty, uri4.Version);
        }
    }
}
