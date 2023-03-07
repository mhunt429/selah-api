#!/bin/sh


# if there is a local_settings.sh file with personal overrides, then load those
if [ -e "./env.sh" ]; then
        echo "Triggering local settings import"
        . ./env.sh 2>&1
fi

dotnet restore

dotnet run --project ./