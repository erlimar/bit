// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

namespace E5R.Tools.Bit.Engine.Compilers
{
    public interface IBitCompiler
    {
        byte[] CompileByteCode(string code);
    }
}
