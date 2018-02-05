# Copyright (c) E5R Development Team. All rights reserved.
# Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

cmake_minimum_required(VERSION 2.8 FATAL_ERROR)

project(bit)

set (bit_SOURCE_DIR "${PROJECT_SOURCE_DIR}/src/E5R.Tools.Bit.Shell")
set (bit_PROJECT_NAME "${CMAKE_PROJECT_NAME}")
set (bit_VERSION_MAJOR 1)
set (bit_VERSION_MINOR 0)
set (bit_VERSION_PATCH 0)
set (bit_VERSION_EXTENSION "dev")

add_subdirectory(src/E5R.Tools.Bit.Shell)
