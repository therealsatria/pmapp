#!/bin/bash

# Make sure the script stops on first error
set -e

# Initialize action variable
ACTION=""

# Function to display help
show_help() {
    echo "Usage: ./reset-database.sh [OPTION]"
    echo "Reset database or remove migrations."
    echo ""
    echo "Options:"
    echo "  -h, --help           Display this help message"
    echo "  -d, --drop           Drop the database"
    echo "  -r, --remove         Remove the last migration"
    echo "  -a, --all            Remove all migrations"
    echo ""
    echo "Examples:"
    echo "  ./reset-database.sh --drop     # Drops the entire database"
    echo "  ./reset-database.sh --remove   # Removes the last migration"
    echo "  ./reset-database.sh --all      # Removes all migrations"
}

# Parse command line arguments
if [ $# -eq 0 ]; then
    show_help
    exit 1
fi

while [ "$1" != "" ]; do
    case $1 in
        -h | --help )           show_help
                                exit 0
                                ;;
        -d | --drop )           ACTION="drop"
                                ;;
        -r | --remove )         ACTION="remove"
                                ;;
        -a | --all )            ACTION="all"
                                ;;
        * )                     echo "Unknown option: $1"
                                show_help
                                exit 1
    esac
    shift
done

# Execute the requested action
if [ "$ACTION" = "drop" ]; then
    echo "WARNING: This will drop the entire database. All data will be lost!"
    read -p "Are you sure you want to continue? (y/N): " confirmation
    confirmation=${confirmation:-N}
    
    if [[ $confirmation =~ ^[Yy]$ ]]; then
        echo "Dropping database..."
        dotnet ef database drop --force --context AppDbContext
        echo "Database dropped successfully."
    else
        echo "Operation cancelled."
        exit 0
    fi
    
elif [ "$ACTION" = "remove" ]; then
    echo "Removing the last migration..."
    dotnet ef migrations remove --context AppDbContext
    echo "Last migration removed successfully."
    
elif [ "$ACTION" = "all" ]; then
    echo "WARNING: This will remove all migrations!"
    read -p "Are you sure you want to continue? (y/N): " confirmation
    confirmation=${confirmation:-N}
    
    if [[ $confirmation =~ ^[Yy]$ ]]; then
        echo "Removing all migrations..."
        # First drop the database to avoid dependency issues
        dotnet ef database drop --force --context AppDbContext
        
        # Remove migration files
        if [ -d "Infrastructures/Data/Migrations" ]; then
            rm -rf Infrastructures/Data/Migrations
            echo "All migrations removed successfully."
        else
            echo "No migrations directory found."
        fi
    else
        echo "Operation cancelled."
        exit 0
    fi
else
    echo "No valid action specified."
    show_help
    exit 1
fi 