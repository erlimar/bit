#!/bin/bash

# Copyright (c) E5R Development Team. All rights reserved.
# Licensed under the Apache License, Version 2.0. More license information in LICENSE.txt.

set -e

SDK_INSTALL_DIR="$PWD/.dotnetsdk"
SDK_PLATFORM="x64"
SDK_VERSION="$CLI_VERSION"
DOWNLOADER=$(which curl)

SCRIPT_INSTALL_URL="https://dot.net/v1/dotnet-install.sh"
SCRIPT_INSTALL_PATH="$SDK_INSTALL_DIR/dotnet-install.sh"

mkdir -p "$SDK_INSTALL_DIR"

if [ ! -f "$SCRIPT_INSTALL_PATH" ]; then
    $DOWNLOADER "$SCRIPT_INSTALL_URL" > "$SCRIPT_INSTALL_PATH"
    chmod +x "$SCRIPT_INSTALL_PATH"
fi

if [ ! -f "$SDK_INSTALL_DIR/dotnet" ]; then
    "$SCRIPT_INSTALL_PATH" --architecture $SDK_PLATFORM --version $SDK_VERSION --install-dir "$SDK_INSTALL_DIR"
fi
