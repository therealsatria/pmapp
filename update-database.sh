#!/bin/bash

# Make sure the script stops on first error
set -e

# Optional parameter for targeting a specific migration
MIGRATION=""
if [ ! -z "$1" ]; then
    MIGRATION=$1
    echo "Updating database to migration: $MIGRATION..."
else
    echo "Updating database to latest migration..."
fi

# Update database
if [ -z "$MIGRATION" ]; then
    dotnet ef database update --context AppDbContext
else
    dotnet ef database update $MIGRATION --context AppDbContext
fi

echo "Database updated successfully!" 