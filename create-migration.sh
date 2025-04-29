#!/bin/bash

# Make sure the script stops on first error
set -e

# Check if migration name is provided
if [ -z "$1" ]; then
    echo "Error: Migration name is required."
    echo "Usage: ./create-migration.sh <MigrationName>"
    exit 1
fi

MIGRATION_NAME=$1

# Create migration
echo "Creating migration: $MIGRATION_NAME..."
dotnet ef migrations add $MIGRATION_NAME --output-dir Infrastructures/Data/Migrations --context AppDbContext

echo "Migration '$MIGRATION_NAME' created successfully!"
echo "To apply this migration to the database, run: ./update-database.sh" 