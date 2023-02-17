# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app-server

# copy csproj and restore as distinct layers
COPY /SoundMastery/SoundMastery.Application/SoundMastery.Application.csproj ./src/SoundMastery.Application/
COPY /SoundMastery/SoundMastery.DataAccess/SoundMastery.DataAccess.csproj ./src/SoundMastery.DataAccess/
COPY /SoundMastery/SoundMastery.Domain/SoundMastery.Domain.csproj ./src/SoundMastery.Domain/
COPY /SoundMastery/SoundMastery.Migration/SoundMastery.Migration.csproj ./src/SoundMastery.Migration/
COPY /SoundMastery/SoundMastery.Api/SoundMastery.Api.csproj ./src/SoundMastery.Api/

# RUN dotnet restore
RUN dotnet restore /app-server/src/SoundMastery.Api/
RUN dotnet restore /app-server/src/SoundMastery.Migration/

# copy everything else and build app
COPY /SoundMastery/. ./src/

WORKDIR /app-server/src/SoundMastery.Api/
RUN dotnet publish -c release -o /app/api --no-restore

WORKDIR /app-server/src/SoundMastery.Migration/
RUN dotnet publish -c release -o /app/migration --no-restore

ARG DB_COMMAND
ENV DB_COMMAND=${DB_COMMAND}

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
COPY entrypoint.sh /scripts/entrypoint.sh
RUN ["chmod", "+x", "/scripts/entrypoint.sh"]
ENTRYPOINT ["/scripts/entrypoint.sh"]