#!/bin/bash
SCRIPTS_FOLDER_PATH="$(realpath "$0" | sed 's|\(.*\)/.*|\1|')"

cd "$SCRIPTS_FOLDER_PATH" && bash ./remove.sh && bash ./init.sh
