// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace E5R.Tools.Bit.Engine
{
    public class EmbeddedResourceLoader
    {
        private Assembly _assembly;
        private string _basePath;

        private EmbeddedResourceLoader(Assembly assembly, string basePath)
        {
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            _basePath = !string.IsNullOrEmpty(basePath)
                ? basePath
                : throw new ArgumentNullException(nameof(basePath));
        }

        public static EmbeddedResourceLoader EmbeddedFrom(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var objType = obj.GetType();
            var assembly = Assembly.GetAssembly(objType);
            return new EmbeddedResourceLoader(assembly, objType.Namespace);
        }

        public EmbeddedResourceLoader Combine(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            string newPath = $"{_basePath}.{path.Replace('/', '.')}";
            return new EmbeddedResourceLoader(_assembly, newPath);
        }

        public Stream Load()
        {
            return _assembly.GetManifestResourceStream(_basePath);
        }

        public ManifestResourceInfo Info(string path)
        {
            return _assembly.GetManifestResourceInfo(path);
        }

        public string[] ResourceNames
        {
            get
            {
                return _assembly.GetManifestResourceNames()
                    .Where(w => w.StartsWith(_basePath))
                    .ToArray();
            }
        }
    }
}
