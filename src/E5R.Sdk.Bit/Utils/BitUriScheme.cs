// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System.ComponentModel;

namespace E5R.Sdk.Bit.Utils
{
    public enum BitUriScheme
    {
        [Description("cmd")]
        Command = 1,

        [Description("lib")]
        Library = 2,

        [Description("doc")]
        Documentation = 3
    }
}
