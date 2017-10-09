// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

namespace System
{
    public static class ByteExtensions
    {
        public static string ToHexadecimalString(this byte[] _this)
        {
            string result = string.Empty;

            foreach (byte b in _this)
            {
                result += b.ToString("x2");
            }

            return result;
        }
    }
}
