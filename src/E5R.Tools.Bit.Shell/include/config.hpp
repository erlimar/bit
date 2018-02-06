// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

/**
 * @note Do not change this file manually.
 *       It is generated automatically by CMake. See the file "config.hpp.in".
 **/
#ifndef _BIT_CONFIG_H_
#define _BIT_CONFIG_H_

#include <string>

#define BIT_ROOT_NAMESPACE_BEGIN namespace E5R { namespace Tools { namespace Bit
#define BIT_ROOT_NAMESPACE_END }}

BIT_ROOT_NAMESPACE_BEGIN
{
    using std::string;
    using std::to_string;

    class Version
    {
    public:
        unsigned int major()
        {
            return 1;
        }

        unsigned int minor()
        {
            return 0;
        }

        unsigned int patch()
        {
            return 0;
        }

        string extension()
        {
            return "dev";
        }

        string semver()
        {
            if ("dev" != "")
            {
                return "1.0.0-dev";
            }

            return "1.0.0";
        }
    };
    
    class Product
    {
    private:
        Product() = default;
        ~Product() = default;

        Product(const Product&) = delete;
        Product(Product&&) = delete;
        Product& operator=(const Product&) = delete;
        Product& operator=(Product&&) = delete;

    public:
        static Product& Info();

        Version version;
        string name();
    };

    Product& Product::Info()
    {
        static Product p;
        return p;
    }

    string Product::name()
    {
        return "bit";
    }
}
BIT_ROOT_NAMESPACE_END

#endif // _BIT_CONFIG_H_
