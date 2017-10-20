// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using System.Threading.Tasks;

namespace E5R.Sdk.Bit.Command
{
    using Services.Abstractions;

    public class BitCommand : IBitCommand
    {
        public virtual Task<BitResult> Main(BitContext context) => throw new NotImplementedException();
    }
}
