// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using Xunit;

namespace E5R.Sdk.Bit.Test.Extensions
{
    using _ = E5R.Test.Commons.TraitConstants;

    [Trait(_.MODULE, "E5R.Sdk.Bit")]
    [Trait(_.COMPONENT, nameof(ByteExtensions))]
    [Trait(_.CATEGORY, "UNIT")]
    public class ByteExtensionsTests
    {
        [Fact(DisplayName = "ToHexadecimalString: Empty array generates empty hexadecimal string")]
        public void ToHexadecimalString_Empty_Array_Generates_Empty_Hexadecimal_String()
        {
            // Arrange
            byte[] emptyArray = new byte[] { };

            // Act
            string result = emptyArray.ToHexadecimalString();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory(DisplayName = "ToHexadecimalString: Generates correct strings for known bytes")]
        [InlineData(new byte[] { 1, 2, 3, 4 }, "01020304")]
        [InlineData(new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, "01010101010101010101")]
        [InlineData(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "00000000000000000000")]
        [InlineData(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 }, "000102030405060708090a0b0c0d0e0f1011121314")]
        public void ToHexadecimalString_Generates_Correct_Strings_For_Known_Bytes(byte[] bytes, string expected)
        {
            Assert.Equal(expected, bytes.ToHexadecimalString());
        }
    }
}
