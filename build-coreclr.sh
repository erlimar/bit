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
JQ_JSON_URL=

# Define variables depending on Linux/OSX
if [[ "$(uname -s)" == "Darwin" ]]; then
    MD5_EXE="md5 -r"
    JQ_JSON_URL="https://github.com/stedolan/jq/releases/download/jq-1.4/jq-osx-x86"
    #JQ_JSON_URL="https://github.com/stedolan/jq/releases/download/jq-1.4/jq-win32.exe"
else
    MD5_EXE="md5sum"
    JQ_JSON_URL="https://github.com/stedolan/jq/releases/download/jq-1.4/jq-linux-x86"
    #JQ_JSON_URL="https://github.com/stedolan/jq/releases/download/jq-1.4/jq-win32.exe"
fi

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

if ! has "$MD5_EXE"; then
    say_error "Tool $MD5_EXE is required and not present!"
    exit 1
fi

if ! has curl; then
    say_error "CURL is required and not present!"
    exit 1
fi

download()
{
    say_verbose "Downloading $1..."
    curl -Lsfo "$2" "$1"
}

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

# TODO: Not Implemented !!!
# function RestorePackages([string] $targetName, [string] $packageFile) {
#     # Check for changes in packages.config and remove installed packages if true.
#     $parentPath = Split-Path $packageFile -Parent
#     $md5File = "$packageFile.md5sum"
#     [string] $md5Hash = MD5HashFile($packageFile)
#
#     if((!(Test-Path $md5File)) -Or ($md5Hash -ne (Get-Content $md5File ))) {
#         Write-Verbose -Message "Missing or changed $targetName package.config hash..."
#         Get-ChildItem -Path $parentPath -Exclude packages.config,Cake.Bakery |
#             Remove-Item -Recurse
#
#         Write-Verbose -Message "Restoring $targetName..."
#         & "$DOTNET_COMMAND" restore $packageFile --packages "$parentPath"
#
#         if ($LASTEXITCODE -ne 0) {
#             Throw "An error occurred while restoring $targetName."
#         }
#         else
#         {
#             $md5Hash | Out-File $md5File -Encoding "ASCII"
#         }
#     }
# }

# TODO: Not Implemented !!!
# # Restore tools?
# if(-Not $SkipToolPackageRestore.IsPresent) {
#     RestorePackages -targetName "tools" -packageFile $TOOLS_PACKAGES_CONFIG
# }

# TODO: Not Implemented !!!
# # Restore addins?
# if(-Not $SkipAddinPackageRestore.IsPresent -and (Test-Path $ADDINS_PACKAGES_CONFIG)) {
#     RestorePackages -targetName "addins" -packageFile $ADDINS_PACKAGES_CONFIG
# }

# TODO: Not Implemented !!!
# # Restore modules?
# if(-Not $SkipModulePackageRestore.IsPresent -and (Test-Path $MODULES_PACKAGES_CONFIG)) {
#     RestorePackages -targetName "modules" -packageFile $MODULES_PACKAGES_CONFIG
# }

# Make sure that Cake has been installed.
if [ ! -f "$CAKE_DLL" ]; then
    say "Could not find Cake.dll at '$CAKE_DLL'."
    exit 1
fi

# Define default arguments.
SCRIPT="build.cake"
CAKE_ARGUMENTS=()

# Build Cake arguments
for i in "$@"; do
    case $1 in
        -s|--script) SCRIPT="$2"; shift ;;
        --) shift; CAKE_ARGUMENTS+=("$@"); break ;;
        *) CAKE_ARGUMENTS+=("$1") ;;
    esac
    shift
done

# Start Cake
say "Running build script..."
exec "$DOTNET_COMMAND" $CAKE_DLL $SCRIPT "${CAKE_ARGUMENTS[@]}"

# # TODO: Not Implemented !!!
# # Make sure that packages.config exist.
# if [ ! -f "$TOOLS_DIR/packages.config" ]; then
#     echo "Downloading packages.config..."
#     curl -Lsfo "$TOOLS_DIR/packages.config" https://cakebuild.net/download/bootstrapper/packages
#     if [ $? -ne 0 ]; then
#         echo "An error occurred while downloading packages.config."
#         exit 1
#     fi
# fi

# # TODO: Not Implemented !!!
# # Download NuGet if it does not exist.
# if [ ! -f "$NUGET_EXE" ]; then
#     echo "Downloading NuGet..."
#     curl -Lsfo "$NUGET_EXE" https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
#     if [ $? -ne 0 ]; then
#         echo "An error occurred while downloading nuget.exe."
#         exit 1
#     fi
# fi

# # Restore tools from NuGet.
# pushd "$TOOLS_DIR" >/dev/null
# if [ ! -f "$PACKAGES_CONFIG_MD5" ] || [ "$( cat "$PACKAGES_CONFIG_MD5" | sed 's/\r$//' )" != "$( $MD5_EXE "$PACKAGES_CONFIG" | awk '{ print $1 }' )" ]; then
#     find . -type d ! -name . ! -name 'Cake.Bakery' | xargs rm -rf
# fi

# mono "$NUGET_EXE" install -ExcludeVersion
# if [ $? -ne 0 ]; then
#     echo "Could not restore NuGet tools."
#     exit 1
# fi

# $MD5_EXE "$PACKAGES_CONFIG" | awk '{ print $1 }' >| "$PACKAGES_CONFIG_MD5"

# popd >/dev/null

# # Restore addins from NuGet.
# if [ -f "$ADDINS_PACKAGES_CONFIG" ]; then
#     pushd "$ADDINS_DIR" >/dev/null

#     mono "$NUGET_EXE" install -ExcludeVersion
#     if [ $? -ne 0 ]; then
#         echo "Could not restore NuGet addins."
#         exit 1
#     fi

#     popd >/dev/null
# fi

# # Restore modules from NuGet.
# if [ -f "$MODULES_PACKAGES_CONFIG" ]; then
#     pushd "$MODULES_DIR" >/dev/null

#     mono "$NUGET_EXE" install -ExcludeVersion
#     if [ $? -ne 0 ]; then
#         echo "Could not restore NuGet modules."
#         exit 1
#     fi

#     popd >/dev/null
# fi
