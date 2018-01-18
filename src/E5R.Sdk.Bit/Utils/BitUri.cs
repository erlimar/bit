// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using System.Linq;
using System.Collections.Generic;

namespace E5R.Sdk.Bit.Utils
{
    public class BitUri
    {
        public BitUriScheme Scheme { get; set; }
        public string Repository { get; set; }
        public string Resource { get; set; }
        public string Version { get; set; }

        public static BitUri ParseOrDefault(string uriString)
        {
            // Ensure [System.Uri] format
            if (!Uri.TryCreate(uriString, UriKind.Absolute, out Uri uri))
            {
                return null;
            }

            var schemes = new Dictionary<string, BitUriScheme>()
            {
                [BitUriScheme.Command.GetDescription()] = BitUriScheme.Command,
                [BitUriScheme.Library.GetDescription()] = BitUriScheme.Library,
                [BitUriScheme.Documentation.GetDescription()] = BitUriScheme.Documentation
            };

            // Scheme in [BitUriScheme]
            if (!schemes.Any(a => a.Key == uri.Scheme))
            {
                return null;
            }

            return new BitUri
            {
                Scheme = schemes[uri.Scheme],
                Resource = string.IsNullOrEmpty(uri.UserInfo)
                    ? uri.Host : uri.UserInfo,
                Repository = string.IsNullOrEmpty(uri.UserInfo)
                    ? string.Empty : uri.Host,
                Version = uri.Segments.FirstOrDefault(f => f != "/")
                    ?? string.Empty
            };
        }
    }
}
