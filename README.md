# SoundMastery ![CI](https://github.com/arublevsky/soundmastery/workflows/CI/badge.svg)

Administration portal for a music school. 

* React SPA
* ASP.NET 6.0 Web API
* Multi database engine support: SQL Server and Postgres.
* Dapper
* ASP.NET Identity with JWT-based authentication
* Docker
* Nuke build automation tool
* GitHub actions CI pipeline on linux

## Prerequsites

1. .NET 6.0.301 SDK
2. SQL Server 2017 or Postgres 13
3. NodeJS 10+
4. docker, docker-compose

## Development

1. Run `./build.ps1` to build the product
2. (optional) Override database engine type in your appsettings.Personal.json for API and Migration projects
```json
  "DatabaseSettings": {
    "Engine": "SqlServer"
  },
```
3. Run `migrate-database.ps1 <command>` to prepare database

    For example:
    `.\migrate-database.ps1 recreate` - to drop and recreate database with seeded users
    `.\migrate-database.ps1 update` - to run new migrations to the existing database

    3a. Run `run.ps1` to start webpack dev server with API host.

    3b. Run `build.ps1 -target DeployDocker` to run the whole application in docker (atm postgress is used as a database engine).
    Note: docker deploy uses production webpack configuration, adjust server URL to point to the local server. See `webpack.prod.js`.

## K8s deployment
Repository contains helm templates to run application in local kubernetes cluster, for exapmple minikube.
To run application in minikube:
1. Intall and configure minikube cluster ([details](https://stackoverflow.com/questions/58561682/minikube-with-ingress-example-not-working/73735009#73735009))
2. Install helm and deploy charts for postgress, client and backend
3. Naviage to http://soundmastery-client.local

## Cloud Deployments

### Azure

Two pre-configured app services:

Client: https://soundmastery-client.azurewebsites.net/
API: https://soundmastery.azurewebsites.net/

Pull-request deployments:

1. Create a pull request and wait for the required checks to complete (so the app is built and images are published)
2. Add `azure-deploy-pull-request` label to run deploy action.
3. When the `AzureDeploy / Deploy PR package (pull_request)` completes, navigate to https://soundmastery-client.azurewebsites.net/ 
