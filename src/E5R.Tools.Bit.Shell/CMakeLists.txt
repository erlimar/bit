# Copyright (c) E5R Development Team. All rights reserved.
# Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

configure_file(
    "${bit_SOURCE_DIR}/include/bit-shell/config.hpp.in"
    "${bit_SOURCE_DIR}/include/bit-shell/config.hpp"
)

include_directories("${bit_SOURCE_DIR}/include")

set(SRCS
    main.cpp
)

add_executable(bit ${SRCS})
