// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

#include <iostream>
#include <string>

#include "config.hpp"

using namespace E5R::Tools::Bit;

using std::cout;
using std::endl;

void main()
{
    cout << Product::Info().name()
         << " v" << Product::Info().version.semver()
         << endl;
}
