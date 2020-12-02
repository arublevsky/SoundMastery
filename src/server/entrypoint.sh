#!/bin/bash
echo "Starting API.."
cd migration
echo "Starting Database migration.."

n="1"

while [ $n -lt 7 ]
do
    echo "Checking the database connectivity.."
    dotnet SoundMastery.Migration.dll check-connection || error_code=$?
    if [ "$error_code" -ne 0 ]; then
        if [ $n -eq 6 ]; then
            echo "Database is not available, exiting"
            exit -1;
        fi
        echo "An attempt #$n to connect to the database failed, retrying in 5 sec.."
        error_code=0
        n=$((n+1))
        sleep 5
    else
        echo "Database available, starting the migration"
        break;
    fi
done

dotnet SoundMastery.Migration.dll ${DB_COMMAND} || error_code=$?

if [ $error_code -ne 0 ]; then
    echo "Database migration failed, exiting.."
    exit $error_code
fi

echo "Database migration finished successfully."
cd ./../api
echo "Starting API server..."
dotnet SoundMastery.Api.dll