#!/bin/bash
# DONT EDIT THIS VALUES
SCRIPTS_FOLDER_PATH="$(realpath "$0" | sed 's|\(.*\)/.*|\1|')"
PROJECT_ROOT_PATH="$(dirname "$SCRIPTS_FOLDER_PATH")"

echo "Stopping and removing db container..."
cd "$PROJECT_ROOT_PATH" && docker-compose down && docker-compose rm -s
