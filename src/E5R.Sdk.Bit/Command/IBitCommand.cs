// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System.Threading.Tasks;

namespace E5R.Sdk.Bit.Command
{
    public interface IBitCommand
    {
        Task<BitCommandResult> Main(BitCommandContext context);
    }
}
