# CONFIGURABLE VARIABLES
DB_CONTAINER_NAME="mysql-db"
DEV_CERTIFICATE_PATH="${HOME}/.aspnet/https/aspnetapp.pfx"

# DONT EDIT THIS VALUES
SETUP_SlEEP_TIME=10
SCRIPTS_FOLDER_PATH="$(realpath "$0" | sed 's|\(.*\)/.*|\1|')"
PROJECT_ROOT_PATH="$(dirname "$SCRIPTS_FOLDER_PATH")"
DATABASE_CONTAINER_SCRIPTS_PATH="$SCRIPTS_FOLDER_PATH/database"
DATABASE_SETUP_SCRIPT_PATH="$DATABASE_CONTAINER_SCRIPTS_PATH/setupDb.sh"

setup_dev_certificate() {

  if test -f "$DEV_CERTIFICATE_PATH"; then
      echo "'$DEV_CERTIFICATE_PATH' already exists."
      return
  fi

  echo "Setting up dev certificate in '$DEV_CERTIFICATE_PATH'"
  dotnet dev-certs https -ep "$DEV_CERTIFICATE_PATH" -p password
  dotnet dev-certs https --trust
  echo "Successfully created dev certificate in '$DEV_CERTIFICATE_PATH'"
}

remove_dangling_volumes () {
    echo "Removing dangling docker volumes..."
    local docker_volumes_string
    local docker_volumes_array
    local docker_volumes_count

    docker_volumes_string="$(docker volume ls -qf dangling=true | xargs)"

    # Split string by whitespace and create array from elements
    IFS=' ' read -r -a docker_volumes_array <<< "$docker_volumes_string"

    docker_volumes_count=${#docker_volumes_array[@]}

    if [ "$docker_volumes_count" -gt "0" ]; then
      echo "Found '$docker_volumes_count' dangling docker volumes."

      # shellcheck disable=SC2046
      # (elements of array have to be separated by a whitespace)
      if docker volume rm $(IFS=' '; echo "${docker_volumes_array[*]}"); then
        echo "Successfully removed '$docker_volumes_count' dangling volumes."
      else
        echo "Failed to remove dangling docker volumes."
      fi
    else
        echo "No dangling docker volumes found."
    fi
}

start_docker_compose () {
    echo "Starting docker compose..."
    if cd "$PROJECT_PATH"; then
      docker compose build --no-cache
      docker compose up -d
    else
      echo "Could not cd to path '$PROJECT_ROOT_PATH'"
    fi
}

setup_database_in_docker_container () {
  echo "Waiting '$SETUP_SlEEP_TIME' seconds for database setup in docker container."
  sleep $SETUP_SlEEP_TIME
  # Arguments
  # 1. DB_CONTAINER_NAME
  # 2. PROJECT_ROOT_PATH
  # 3. DB_SCRIPTS_PATH
  bash "$DATABASE_SETUP_SCRIPT_PATH" "$DB_CONTAINER_NAME" "$PROJECT_ROOT_PATH" "$DATABASE_CONTAINER_SCRIPTS_PATH"
}

printf "[ Setup dev certificate ]\n"
if ! setup_dev_certificate; then
  echo "ERROR: Failed to set up dev certificate."
  exit 1
fi

printf "\n\n"
printf "[ Remove dangling docker images ]\n"
if ! remove_dangling_volumes; then
  echo "ERROR: Failed to remove dangling docker volumes."
  exit 1
fi

printf "\n\n"
printf "[ Start docker compose configuration ]\n"
if ! start_docker_compose; then
  echo "ERROR: Failed to start docker compose configuration."
  exit 1
fi

printf "\n\n"
printf "[ Set up database in docker container ]\n"
if ! setup_database_in_docker_container; then
  echo "ERROR: Failed to set up database in docker container."
  exit 1
fi

printf "\nSuccessfully initialized environment."
