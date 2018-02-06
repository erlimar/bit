# Copyright (c) E5R Development Team. All rights reserved.
# Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

# Requeremos a versão 3.6 do CMake porque somente à partir
# dessa versão temos binários disponíveis para Windows, Linux
# e macOS nas duas plataformas x86 e x64.
cmake_minimum_required(VERSION 3.6 FATAL_ERROR)

project(bit)

# C/C++ standards
SET(CMAKE_CXX_STANDARD 11)
SET(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

# Tool variables configuration
set (bit_SOURCE_DIR "${PROJECT_SOURCE_DIR}/src/E5R.Tools.Bit.Shell")
set (bit_PROJECT_NAME "${CMAKE_PROJECT_NAME}")
set (bit_VERSION_MAJOR 1)
set (bit_VERSION_MINOR 0)
set (bit_VERSION_PATCH 0)
set (bit_VERSION_EXTENSION "dev")

# CPack configuration
#include(InstallRequiredSystemLibraries)
set(CPACK_RESOURCE_FILE_LICENSE "${CMAKE_CURRENT_SOURCE_DIR}/LICENSE.txt")
set(CPACK_PACKAGE_VERSION_MAJOR ${bit_VERSION_MAJOR})
set(CPACK_PACKAGE_VERSION_MINOR ${bit_VERSION_MINOR})
set(CPACK_PACKAGE_VERSION_PATCH ${bit_VERSION_PATCH})

#set(LIBCURL_PATH "${CMAKE_CURRENT_SOURCE_DIR}/build/.curl/install")
#set(CMAKE_TOOLCHAIN_FILE "${CMAKE_CURRENT_SOURCE_DIR}/build/.curl/install/CMake/libcurl-target-release.cmake")
#libcurl-target.cmake
#curl-config-version.cmake
#curl-config.cmake
message("${CMAKE_CURRENT_SOURCE_DIR}/build/.curl/install/CMake")
list(APPEND CMAKE_MODULE_PATH "${CMAKE_CURRENT_SOURCE_DIR}/build/.curl/install/CMake")

include(FindCURL)
find_package(CURL)

if(CURL_FOUND)
  message(STATUS "Found CURL version: ${CURL_VERSION_STRING}")
  message(STATUS "Using CURL include dir(s): ${CURL_INCLUDE_DIRS}")
  message(STATUS "Using CURL lib(s): ${CURL_LIBRARIES}")
else()
  message(FATAL_ERROR "Could not find CURL")
endif()

# Target components
add_subdirectory(src/E5R.Tools.Bit.Shell)

include(CPack)
