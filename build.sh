#!/usr/bin/env bash

set -Eeuo pipefail

# Hayward runs on .NET 8
if ! command -v dotnet &> /dev/null; then
  echo "The ``dotnet`` CLI could not be found. Please install the .NET 8 SDK and try again."
  exit 1
fi

# Expect .NET 8
DOTNET_VERSION="$(dotnet --version || true)"
DOTNET_VERSION_MAJOR="${DOTNET_VERSION%%.*}"

if [[ "$DOTNET_VERSION_MAJOR" -lt 8 ]]; then
  echo "Hayward requires .NET 8 or higher. Found: $DOTNET_VERSION"
  exit 1
fi

SOLUTION_PATH="./src/hayward.sln"
OUTPUT_DIR="bin"

echo "Publishing solution..."
dotnet publish "$SOLUTION_PATH" -c Release -o "$OUTPUT_DIR"

if [ -d "src/bin" ]; then
  rm -rf "src/bin"
  echo "Removed src/bin"
fi

if [ -d "src/obj" ]; then
  rm -rf "src/obj"
  echo "Removed src/obj"
fi

if [ -d "obj" ]; then
  rm -rf "obj"
  echo "Removed obj"
fi

echo "Try running ``./bin/hayward -h`` and happy coding!"
