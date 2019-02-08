#!/usr/bin/env bash

##########################################################################
# This is the Cake bootstrapper script for Linux and OS X.
# This file was downloaded from https://github.com/cake-build/resources
# Feel free to change this file to fit your needs.
##########################################################################

#Default DotNet SDK minimal version 1.1.4
SDK_VERSION_CAKE="1.0.4"
SDK_VERSION_GLOBAL=$SDK_VERSION_CAKE

# Define directories.
SCRIPT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
BUILD_DIR="$SCRIPT_DIR/build"
TOOLS_DIR="$BUILD_DIR/tools"
ADDINS_DIR="$BUILD_DIR/addins"
MODULES_DIR="$BUILD_DIR/modules"
DOTNET_DIR="$BUILD_DIR/.dotnetsdk"
DOTNET_INSTALL_URL="https://dot.net/v1/dotnet-install.sh"
DOTNET_INSTALL_PATH="$DOTNET_DIR/dotnet-install.sh"
DOTNET_COMMAND="$DOTNET_DIR/dotnet"
DOTNET_RUNTIME_CAKE_PATH="$DOTNET_DIR/shared/Microsoft.NETCore.App/$SDK_VERSION_CAKE"
CAKE_VERSION="0.23.0"
CAKE_DLL="$TOOLS_DIR/cake.coreclr/$CAKE_VERSION/Cake.dll"
TOOLS_PACKAGES_CONFIG="$TOOLS_DIR/packages.config"
ADDINS_PACKAGES_CONFIG="$ADDINS_DIR/packages.config"
MODULES_PACKAGES_CONFIG="$MODULES_DIR/packages.config"
GLOBAL_JSON_PATH="$SCRIPT_DIR/global.json"
JQ_JSON_EXE="$TOOLS_DIR/jq"
MD5_EXE=
MD5_FLAG
JQ_JSON_URL=

has() {
    hash "$1" > /dev/null 2>&1
    return $?
}

say() {
    printf "%s\n" "$1"
}

say_verbose() {
    printf "verbose: %s\n" "$1"
}

say_error() {
    printf "error: %s\n" "$1"
}

download()
{
    say_verbose "Downloading $1..."
    curl -Lsfo "$2" "$1"
}

write_cake_tools_dotnet_project() {
cat > "$1" <<- EOF
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <TargetFramework>netstandard1.6</TargetFramework>
    </PropertyGroup>
    <ItemGroup>
        <PackageReference Include="Cake.CoreCLR" Version="$CAKE_VERSION" />
    </ItemGroup>
</Project>
EOF
}

restore_packages() {
    local TARGET_NAME=$1
    local PACKAGE_FILE=$2

    # Check for changes in packages.config and remove installed packages if true.
    local PARENT_PATH=$(dirname "$PACKAGE_FILE")
    local MD5_FILE="$PACKAGE_FILE.md5sum"
    local CALCULATED_MD5=
    local STORED_MD5=

    if [ -f "$MD5_FILE" ]; then
        CALCULATED_MD5=$($MD5_EXE $MD5_FLAG "$PACKAGE_FILE" | awk '{ print $1 }')
        STORED_MD5=$(cat "$MD5_FILE" | sed 's/\r$//')
    fi

    if [ ! -f "$MD5_FILE" ] || [ "$STORED_MD5" != "$CALCULATED_MD5" ]; then
        say_verbose "Missing or changed $TARGET_NAME package.config hash..."
        pushd "$PARENT_PATH" >/dev/null
        find . -type d ! -name . ! -name 'Cake.Bakery' | xargs rm -rf
        popd >/dev/null

        say_verbose "Restoring $TARGET_NAME..."
        "$DOTNET_COMMAND" restore $PACKAGE_FILE --packages "$PARENT_PATH"

        if [ $? -ne 0 ]; then
            say_error "An error occurred while restoring $TARGET_NAME."
            return 1
        else
            echo "$CALCULATED_MD5" | awk '{ print $1 }' >| "$MD5_FILE"
        fi
    fi
}

# Define variables depending on Linux/OSX
if [[ "$(uname -s)" == "Darwin" ]]; then
    MD5_EXE="md5"
    MD5_FLAG="-r"
    JQ_JSON_URL="https://github.com/stedolan/jq/releases/download/jq-1.4/jq-osx-x86"
else
    MD5_EXE="md5sum"
    JQ_JSON_URL="https://github.com/stedolan/jq/releases/download/jq-1.4/jq-linux-x86"
fi

if ! has "$MD5_EXE"; then
    say_error "Tool $MD5_EXE is required and not present!"
    exit 1
fi

if ! has curl; then
    say_error "CURL is required and not present!"
    exit 1
fi

# Ensure jq: https://stedolan.github.io/jq/download/
if has jq; then
    JQ_JSON_EXE=jq
fi

if ! has jq; then
if [ ! -f "$JQ_JSON_EXE" ]; then
    download "$JQ_JSON_URL" "$JQ_JSON_EXE"
    
    if [ ! -f "$JQ_JSON_EXE" ]; then
        say_error "Tool JQ is required, not present and not downloaded!"
        exit 1
    fi

    chmod +x "$JQ_JSON_EXE"
fi
fi

# Detecting global.json
if [ -f "$GLOBAL_JSON_PATH" ]; then
    say_verbose "Detecting .NET SDK version from global.json in $GLOBAL_JSON_PATH"
    GLOBAL_JSON_VERSION=`cat "$GLOBAL_JSON_PATH" | exec "$JQ_JSON_EXE" -r '.sdk.version'`

    if [[ "$GLOBAL_JSON_VERSION" != "" ]]; then
        SDK_VERSION_GLOBAL=$GLOBAL_JSON_VERSION
        say_verbose "Detected .NET SDK version: $SDK_VERSION_GLOBAL"
    else
        say_verbose "File global.json don't contain sdk version information"
    fi
fi

say "Preparing to run build script..."

# Make sure the tools folder exist.
if [ ! -d "$TOOLS_DIR" ]; then
    mkdir "$TOOLS_DIR"
fi

# Make sure that packages.config exist.
if [ ! -f "$TOOLS_PACKAGES_CONFIG" ]; then
    say_verbose "Writing packages.config..."    
    write_cake_tools_dotnet_project "$TOOLS_PACKAGES_CONFIG"

    if [ ! -f "$TOOLS_PACKAGES_CONFIG" ]; then
        say_error "Could not write packages.config."
        return 1
    fi
fi

# Try install DotNet if not exists
if [ ! -f "$DOTNET_COMMAND" ]; then
    say "Installing DotNet SDK v$SDK_VERSION_GLOBAL..."
    mkdir -p "$DOTNET_DIR"
   
    if [ ! -f "$DOTNET_INSTALL_PATH" ]; then
        say_verbose "Downloading DotNet installer..."
        download "$DOTNET_INSTALL_URL" "$DOTNET_INSTALL_PATH"

        if [ ! -f "$DOTNET_INSTALL_PATH" ]; then
            say_error "Could not download DotNet installer."
            return 1
        fi

        chmod +x "$DOTNET_INSTALL_PATH"
    fi

    "$DOTNET_INSTALL_PATH" --version "$SDK_VERSION_GLOBAL" --install-dir "$DOTNET_DIR"

    if [ ! -f "$DOTNET_COMMAND" ]; then
        say_error "Could not install DotNet $SDK_VERSION_GLOBAL."
        return 1
    fi
fi

# Ensure DotNet runtime for Cake tool
if [ ! -d "$DOTNET_RUNTIME_CAKE_PATH" ]; then
    say "Installing DotNet Runtime v$SDK_VERSION_CAKE for Cake tool..."
    mkdir -p "$DOTNET_DIR"
   
    if [ ! -f "$DOTNET_INSTALL_PATH" ]; then
        say_verbose "Downloading DotNet installer..."
        download "$DOTNET_INSTALL_URL" "$DOTNET_INSTALL_PATH"

        if [ ! -f "$DOTNET_INSTALL_PATH" ]; then
            say_error "Could not download DotNet installer."
            return 1
        fi

        chmod +x "$DOTNET_INSTALL_PATH"
    fi

    "$DOTNET_INSTALL_PATH" --shared-runtime --version "$SDK_VERSION_CAKE" --install-dir "$DOTNET_DIR"

    if [ ! -d "$DOTNET_RUNTIME_CAKE_PATH" ]; then
        say_error "Could not install DotNet Runtime v$SDK_VERSION_GLOBAL for Cake tool."
        return 1
    fi
fi

if [ -f "$DOTNET_INSTALL_PATH" ]; then
    rm "$DOTNET_INSTALL_PATH"
fi

# Add dotnet path to PATH environment to be available to child processed
export PATH="$DOTNET_DIR:$PATH"

# Define default arguments.
SCRIPT="build.cake"
SKIP_TOOLS_RESTORE=
SKIP_ADDIN_RESTORE=
SKIP_MODULE_RESTORE=
CAKE_ARGUMENTS=()

# Build Cake arguments
for i in "$@"; do
    case $1 in
        -s|--script) SCRIPT="$2"; shift ;;
        --skip-tools-restore) SKIP_TOOLS_RESTORE=1;;
        --skip-addin-restore) SKIP_ADDIN_RESTORE=1;;
        --skip-module-restore) SKIP_MODULE_RESTORE=1;;
        --) shift; CAKE_ARGUMENTS+=("$@"); break ;;
        *) CAKE_ARGUMENTS+=("$1") ;;
    esac
    shift
done

# Restore tools?
if [ "$SKIP_TOOLS_RESTORE" == "" ]; then
    restore_packages "tools" "$TOOLS_PACKAGES_CONFIG"
fi

# Restore addins?
if [ "$SKIP_ADDIN_RESTORE" == "" ]; then
    if [ -d "$ADDINS_PACKAGES_CONFIG" ]; then
        restore_packages "addins" "$ADDINS_PACKAGES_CONFIG"
    fi
fi

# Restore modules?
if [ "$SKIP_MODULE_RESTORE" == "" ]; then
    if [ -d "$MODULES_PACKAGES_CONFIG" ]; then
        restore_packages "modules" "$MODULES_PACKAGES_CONFIG"
    fi
fi

# Make sure that Cake has been installed.
if [ ! -f "$CAKE_DLL" ]; then
    say "Could not find Cake.dll at '$CAKE_DLL'."
    exit 1
fi

# Start Cake
say "Running build script..."
exec "$DOTNET_COMMAND" $CAKE_DLL $SCRIPT "${CAKE_ARGUMENTS[@]}"
