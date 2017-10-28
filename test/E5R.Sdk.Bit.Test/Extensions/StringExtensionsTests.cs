// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using Xunit;

namespace E5R.Sdk.Bit.Test.Extensions
{
    using _ = E5R.Test.Commons.TraitConstants;

    [Trait(_.MODULE, "E5R.Sdk.Bit")]
    [Trait(_.COMPONENT, nameof(StringExtensions))]
    [Trait(_.CATEGORY, "UNIT")]
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

        #region Hash
        [Theory(DisplayName = "Empty string returns null value for all algorithm.")]
        [InlineData("")]
        [InlineData("     ")]
        public void Empty_String_Returns_Null_Value(string s)
        {
            Assert.Null(s.Md5());
            Assert.Null(s.Sha1());
            Assert.Null(s.Sha256());
            Assert.Null(s.Sha384());
            Assert.Null(s.Sha512());
        }

        [Theory(DisplayName = "Empty hashName parameter up ArgumentNullException for all algorithm.")]
        [InlineData(null)]
        [InlineData("")]
        public void Empty_HashName_Up_ArgumentNullException(string hashName)
        {
            Assert.Throws<ArgumentNullException>(() => "a".ToHash(hashName));
        }

        [Theory(DisplayName = "Invalid hashName parameter up NotSupportedException for all algorithm.")]
        [InlineData(" MD5 ")]
        [InlineData(" SHA1 ")]
        [InlineData(" SHA256 ")]
        [InlineData(" SHA384 ")]
        [InlineData(" SHA512 ")]
        [InlineData("no")]
        [InlineData("invalid")]
        public void Invalid_HashName_Up_NotSupportedException(string hashName)
        {
            Assert.Throws<NotSupportedException>(() => "a".ToHash(hashName));
        }

        [Theory(DisplayName = "No case sensitive algorithm name.")]
        [InlineData("md5")]
        [InlineData("mD5")]
        [InlineData("Md5")]
        [InlineData("MD5")]
        [InlineData("sha1")]
        [InlineData("Sha1")]
        [InlineData("sHa1")]
        [InlineData("shA1")]
        [InlineData("SHA1")]
        [InlineData("sha256")]
        [InlineData("Sha256")]
        [InlineData("sHa256")]
        [InlineData("shA256")]
        [InlineData("SHA256")]
        [InlineData("sha384")]
        [InlineData("Sha384")]
        [InlineData("sHa384")]
        [InlineData("shA384")]
        [InlineData("SHA384")]
        [InlineData("sha512")]
        [InlineData("Sha512")]
        [InlineData("sHa512")]
        [InlineData("shA512")]
        [InlineData("SHA512")]
        public void No_Case_Sensitive(string hashName)
        {
            Assert.NotEmpty("a".ToHash(hashName));
        }

        [Theory(DisplayName = "Hash algorithm has string length fixed")]
        [InlineData("a")]
        [InlineData("All values")]
        [InlineData("E5R Development Team")]
        public void String_Length_Is_Fixed(string value)
        {
            Assert.Equal(32, value.Md5().Length);
            Assert.Equal(40, value.Sha1().Length);
            Assert.Equal(64, value.Sha256().Length);
            Assert.Equal(96, value.Sha384().Length);
            Assert.Equal(128, value.Sha512().Length);
        }
        #endregion Hash
    }
}
