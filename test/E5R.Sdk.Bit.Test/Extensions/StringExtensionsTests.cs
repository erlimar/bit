// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using Xunit;

namespace E5R.Sdk.Bit.Test.Extensions
{
    using _ = E5R.Test.Commons.TraitConstants;

    [Trait(_.MODULE, "E5R.Sdk.Bit")]
    [Trait(_.COMPONENT, nameof(StringExtensions))]
    public class StringExtensionsTests
    {
        #region Base64
        [Fact(DisplayName = "ToBase64: Empty generates null")]
        public void ToBase64_Of_Empty_Generates_Null()
        {
            Assert.Null(string.Empty.ToBase64());
            Assert.Null("".ToBase64());
            Assert.Null("           ".ToBase64());
        }

        [Fact(DisplayName = "ToBase64: Not empty generates not null")]
        public void ToBase64_Of_Not_Empty_Generates_Not_Null()
        {
            Assert.NotNull("a".ToBase64());
        }

        [Theory(DisplayName = "ToBase64: Encode valid strings")]
        [InlineData("base64", "YgBhAHMAZQA2ADQA")]
        [InlineData("Base64", "QgBhAHMAZQA2ADQA")]
        [InlineData("Erlimar", "RQByAGwAaQBtAGEAcgA=")]
        [InlineData("E5R Development Team\nhttp://e5r.org", "RQA1AFIAIABEAGUAdgBlAGwAbwBwAG0AZQBuAHQAIABUAGUAYQBtAAoAaAB0AHQAcAA6AC8ALwBlADUAcgAuAG8AcgBnAA==")]
        public void ToBase64_Encode_Valid_Strings(string rawString, string expected)
        {
            Assert.Equal(expected, rawString.ToBase64());
        }

        [Fact(DisplayName = "FromBase64: Empty generates null")]
        public void FromBase64_Of_Empty_Generates_Null()
        {
            Assert.Null(string.Empty.FromBase64());
            Assert.Null("".FromBase64());
            Assert.Null("           ".FromBase64());
        }

        [Theory(DisplayName = "FromBase64: Detect invalid format")]
        [InlineData("a")]
        [InlineData("aaa")]
        [InlineData("aaaaa")]
        [InlineData(".\n")]
        [InlineData(".\r")]
        [InlineData(".\t")]
        [InlineData("ab d")]
        [InlineData("abc===")]
        public void FromBase64_Detect_Invalid_Format(string invalidBase64String)
        {
            Assert.Throws<FormatException>(() => invalidBase64String.FromBase64());
        }

        [Theory(DisplayName = "IsBase64String: Detect invalid format")]
        [InlineData("a")]
        [InlineData("aaa")]
        [InlineData("aaaaa")]
        [InlineData(".\n")]
        [InlineData(".\r")]
        [InlineData(".\t")]
        [InlineData("ab d")]
        [InlineData("abc===")]
        public void IsBase64String_Detect_Invalid_Format(string invalidBase64String)
        {
            Assert.False(invalidBase64String.IsBase64String());
        }

        [Theory(DisplayName = "FromBase64: Dencode valid strings")]
        [InlineData("base64", "YgBhAHMAZQA2ADQA")]
        [InlineData("Base64", "QgBhAHMAZQA2ADQA")]
        [InlineData("Erlimar", "RQByAGwAaQBtAGEAcgA=")]
        [InlineData("E5R Development Team\nhttp://e5r.org", "RQA1AFIAIABEAGUAdgBlAGwAbwBwAG0AZQBuAHQAIABUAGUAYQBtAAoAaAB0AHQAcAA6AC8ALwBlADUAcgAuAG8AcgBnAA==")]
        public void FromBase64_Decode_Valid_Strings(string expected, string base64Value)
        {
            Assert.Equal(expected, base64Value.FromBase64());
        }
        #endregion Base64
    }
}
