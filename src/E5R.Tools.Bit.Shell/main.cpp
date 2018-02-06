// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

#include <iostream>
#include <string>

#include "e5r/tools/bit.hpp"

using namespace E5R::Tools;

void main()
{
    std::cout
        << Bit::Product::Info().name()
        << " v" << Bit::Product::Info().version.semver()
        << std::endl;
}
