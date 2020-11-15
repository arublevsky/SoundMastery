# SoundMastery ![CI](https://github.com/arublevsky/soundmastery/workflows/CI/badge.svg)

Portfolio project. 

* React SPA
* ASP.NET Core 3.1 Web API
* Multi database engine support: SQL Server and Postgress.
* Dapper
* ASP.NET Identity with JWT-based authentication
* Docker
* Nuke build automation tool
* GitHub actions CI pipeline on linux

## Prerequsites

1. .NET Core 3.1.2
2. SQL Server 2017 or Postgres 13
3. NodeJS 10+

## Development

1. Run `./build.ps1` to build the product
2. (optional) Override database engine type in your appsettings.Personal.json for API and Migration projects
```json
  "DatabaseSettings": {
    "Engine": "SqlServer"
  },
```
3. Run `migrate-database.ps1 <command>` to prepare database

For example
`.\migrate-database.ps1 recreate` - to drop and recreate database with seeded users
`.\migrate-database.ps1 update` - to run new migrations to the existing database

3a. Run `run.ps1` to start webpack dev server with API host.

3b. Run `build.ps1 -target DeployDocker` to run the whole application in docker (atm postgress is used as a database engine)

4. Setup SSL certificate

In Powershell, execute the following command:

`Import-Certificate -FilePath "C:/<path-to-project>SoundMastery/tools/ssl/private.crt" -CertStoreLocation Cert:\LocalMachine\Root`

To generate a new certificate use [this](https://gist.github.com/pgilad/63ddb94e0691eebd502deee207ff62bd) guide.
