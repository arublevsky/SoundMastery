# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app-server

# copy csproj and restore as distinct layers
COPY /server/SoundMastery/SoundMastery.Application/SoundMastery.Application.csproj ./src/SoundMastery.Application/
COPY /server/SoundMastery/SoundMastery.DataAccess/SoundMastery.DataAccess.csproj ./src/SoundMastery.DataAccess/
COPY /server/SoundMastery/SoundMastery.Domain/SoundMastery.Domain.csproj ./src/SoundMastery.Domain/
COPY /server/SoundMastery/SoundMastery.Migration/SoundMastery.Migration.csproj ./src/SoundMastery.Migration/
COPY /server/SoundMastery/SoundMastery.Api/SoundMastery.Api.csproj ./src/SoundMastery.Api/

# RUN dotnet restore
RUN dotnet restore /app-server/src/SoundMastery.Api/

# copy everything else and build app
COPY server/SoundMastery/. ./src/

WORKDIR /app-server/src/SoundMastery.Api/
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "SoundMastery.Api.dll"]