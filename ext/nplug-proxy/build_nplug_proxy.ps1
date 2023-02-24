#! /usr/bin/env pwsh
#
# Build script for Steinberg VST validator shared library, building the following platforms:
#
# On Windows:
#   win-x64
#   win-arm64
#
# On Linux:
#   linux-x64
#   linux-arm64
#
# On Mac:
#   osx-x64
#   osx-arm64
# -------------------------------------------------------------
$ErrorActionPreference = "Stop"


$cmakeConfig = "Release"
#$cmakeConfig = "RelWithDebInfo"
#$cmakeConfig = "Debug"

& "$PSScriptRoot/../CMake-Build-Platforms.ps1" -bit32 $false -BuildFolder "build" -CMakeConfig $cmakeConfig