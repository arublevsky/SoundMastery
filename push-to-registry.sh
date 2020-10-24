#!/bin/bash
dotnet tool restore
VERSION=$(dotnet minver)
echo $'\n'"APP_VERSION=$VERSION" >> .config/ci.env
echo $'\n'"SA_PASSWORD=${SA_PASSWORD}" >> .config/ci.env
echo $'\n'"DB_COMMAND=${DB_COMMAND}" >> .config/ci.env
docker login docker.pkg.github.com -u ${GITHUB_ACTOR} -p ${GITHUB_TOKEN}
docker-compose -f ./tools/docker/docker-compose.ci.yml --env-file .config/ci.env build
docker-compose -f ./tools/docker/docker-compose.ci.yml --env-file .config/ci.env push