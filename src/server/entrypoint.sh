#!/bin/bash
echo "Starting API.."
echo "Sleep for a while to ensure SQL Server is up and running"
sleep 10
echo "Starting Database migration.."
cd migration
dotnet SoundMastery.Migration.dll ${DB_COMMAND}  || error_code=$?
if [ $error_code -ne 0 ]; then
    echo "Database migration failed, exiting.."
    exit $error_code
fi
echo "Database migration finished successfully."
cd ./../api
echo "Srarting API server..."
dotnet SoundMastery.Api.dll