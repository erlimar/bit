// Copyright (c) E5R Development Team. All rights reserved.
// Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

#include <iostream>
#include <string>

#include "bit-shell/config.hpp"

using namespace std;

void main()
{
    string version;

    version += to_string(BIT_VERSION_MAJOR);
    version += "." + to_string(BIT_VERSION_MINOR);
    version += "." + to_string(BIT_VERSION_PATCH);

    if (BIT_VERSION_EXTENSION != "")
    {
        version += "-" BIT_VERSION_EXTENSION;
    }

    std::cout << BIT_PROJECT_NAME
              << " v" << version << std::endl;
}
