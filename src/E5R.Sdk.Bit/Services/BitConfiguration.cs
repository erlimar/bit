// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System.Text;

namespace E5R.Sdk.Bit.Services
{
    using Abstractions;

    public class BitConfiguration : IBitConfiguration
    {
        public Encoding DefaultEncoding => Encoding.UTF8;
    }
}
