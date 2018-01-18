// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

namespace System
{
    using Reflection;
    using Linq;
    using ComponentModel;

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum en)
        {
            string name = en.ToString();

            var info = en.GetType().GetMember(name)
                .FirstOrDefault()?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .FirstOrDefault() as DescriptionAttribute;

            return info?.Description ?? name;
        }
    }
}
