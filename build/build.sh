#!/bin/bash

# Copyright (c) E5R Development Team. All rights reserved.
# Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

set -e

SDK_INSTALL_DIR="$PWD/.dotnetsdk"
echo SDK_INSTALL_DIR="$PWD/.dotnetsdk"
DOTNET_EXE="$SDK_INSTALL_DIR/dotnet"
echo DOTNET_EXE="$SDK_INSTALL_DIR/dotnet"
PACKAGE_VERSION="$VERSION-dev-b-$TRAVIS_BUILD_NUMBER"
echo PACKAGE_VERSION="$VERSION-dev-b-$TRAVIS_BUILD_NUMBER"

echo "$DOTNET_EXE" --info
"$DOTNET_EXE" --info
"$DOTNET_EXE" restore --no-cache
"$DOTNET_EXE" build -c $CONFIGURATION

"$DOTNET_EXE" pack "./src/E5R.Sdk.Bit/E5R.Sdk.Bit.csproj" -c $CONFIGURATION /p:VersionPrefix="$PACKAGE_VERSION"
