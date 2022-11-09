#!/bin/bash
DB_SCRIPT_PATH=$1

awk '{gsub("ENGINE = InnoDB", "", $0); print}' $DB_SCRIPT_PATH > temp.sql && mv temp.sql $DB_SCRIPT_PATH
awk '{gsub(" VISIBLE", "", $0); print}' $DB_SCRIPT_PATH > temp.sql && mv temp.sql $DB_SCRIPT_PATH