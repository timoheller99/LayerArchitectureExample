#!/bin/bash
# CONFIGURABLE VARIABLES
SQL_SCRIPTS=("database.sql" "initialize.sql")

# DONT EDIT THIS VALUES
DB_CONTAINER_NAME=$1
PROJECT_ROOT_PATH=$2
DB_SCRIPTS_PATH=$3

EXECUTE_SQL_SCRIPT_IN_CONTAINER_SCRIPT_PATH="$DB_SCRIPTS_PATH/executeSqlScriptInContainer.sh"
FIX_DB_SCRIPT_PATH="$DB_SCRIPTS_PATH/fixSqlScript.sh"

DB_FOLDER_PATH="$PROJECT_ROOT_PATH/database/createScripts/MySql"

fix_sql_script() {
  local script_path=$1

  echo "[ Fixing '${script_path}' ]"

  if ! test -f "$script_path"; then
    echo "File '$script_path' doesn't exist."
    return
  fi

  bash "$FIX_DB_SCRIPT_PATH" "$script_path"

  echo "Successfully fixed SQL script '${script_path}'."
}

execute_script_in_container() {
  local script_path=$1
  local script_name="${script_path##*/}"

  echo "[ Execute '${script_name}' in docker container '${DB_CONTAINER_NAME}' ]"

  if ! test -f "$script_path"; then
    echo "File '$script_path' doesn't exist."
    return
  fi

  if bash "$EXECUTE_SQL_SCRIPT_IN_CONTAINER_SCRIPT_PATH" "$DB_CONTAINER_NAME" "$script_path" "$script_name"; then
    echo "Successfully executed script '$script_name' in container '$DB_CONTAINER_NAME'."
  else
    echo "Failed to execute script '$script_name' in container '$DB_CONTAINER_NAME'."
  fi
}

for sql_script_name in "${SQL_SCRIPTS[@]}"
do
  printf "\n\n"
  echo "[ Processing $sql_script_name ]"
  fix_sql_script "$DB_FOLDER_PATH/$sql_script_name"
  printf "\n\n"
  execute_script_in_container "$DB_FOLDER_PATH/$sql_script_name"
done
