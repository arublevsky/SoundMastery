Echo "Starting application in docker"
cd ./tools/docker
docker-compose --env-file ./../../config/.env.dev up --build  