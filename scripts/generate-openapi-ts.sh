#!/bin/bash

# Description:
#
# Usage:
#   ./generate-openapi-ts.sh [-i <openapi-spec-url>] [-c <frontend-dir>] [-o <output-file>]
#
# Options:
#   -i <openapi-spec-url>  URL of the OpenAPI spec (default: http://localhost:5222/openapi/spec.json)
#   -c <frontend-dir>     Path to the frontend directory (default: ../frontend)
#   -o <output-file>      Path to the output file (default: ./shared/api-clients/split-the-bill-api/spec.d.ts)
#                           NOTE: this is relative to the frontend directory
# 
# Example:
#   ./generate-openapi-ts.sh \
#       -i http://localhost:5222/openapi/spec.json \
#       -c ../frontend \
#       -o ./shared/api-clients/split-the-bill-api/spec.d.ts
#
# Documentation:
#   - https://openapi-ts.dev/introduction
#   - https://openapi-ts.dev/openapi-fetch/
#
# Notes:
#   - The script expects the frontend folder to have all Node.js dependencies installed (both normal and dev).
#
# Dependencies
#   - Node.js
#   - nvm preconfigured with the version specified in the frontend's .nvmrc file

# Function to display usage information
usage() {
    echo "Usage: $0 [-i <openapi-spec-url>] [-c <frontend-dir>] [-o <output-file>]"
    echo "  -i  OpenAPI spec URL (default: http://localhost:5222/openapi/spec.json)"
    echo "  -c  Frontend directory (default: ../frontend)"
    echo "  -o  Output file (relative to frontend directory, default: ./shared/api-clients/split-the-bill-api/spec.d.ts)"
    exit 1
}

# Default values
OPENAPI_SPEC_URL="http://localhost:5222/openapi/spec.json"
FRONTEND_DIR="../frontend"
OUTPUT_FILE="./shared/api-clients/split-the-bill-api/spec.d.ts"

# Parse command line arguments
while getopts "i:c:o:" opt; do
    case ${opt} in
        i)
            OPENAPI_SPEC_URL=$OPTARG
            ;;
        c)
            FRONTEND_DIR=$OPTARG
            ;;
        o)
            OUTPUT_FILE=$OPTARG
            ;;
        *)
            usage
            ;;
    esac
done
shift $((OPTIND - 1))

echo "Generating TypeScript types from OpenAPI spec"
echo "OpenAPI spec URL: $OPENAPI_SPEC_URL"
echo "Frontend directory: $FRONTEND_DIR"
echo "Output file: $OUTPUT_FILE"

# Change directory to the frontend folder (sibling of the current project)
pushd "$FRONTEND_DIR" > /dev/null || { echo "Frontend folder not found"; exit 1; }

nvm use
npx openapi-typescript "$OPENAPI_SPEC_URL" --output "$OUTPUT_FILE"

popd > /dev/null
echo "Generated TypeScript types from OpenAPI spec and saved to $OUTPUT_FILE"