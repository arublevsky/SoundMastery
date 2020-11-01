# SoundMastery ![CI](https://github.com/arublevsky/soundmastery/workflows/CI/badge.svg)

Portfolio project. 

* React SPA
* ASP.NET Core Web API
* Multi database engine support: SQL Server and Postgress.
* Dapper
* ASP.NET Identity
* Docker
* Nuke build automation tool
* GitHub actions CI pipeline on linux

## Prerequsites

1. .NET Core 3.1.2
2. SQL Server 2017 or Postgres 13
3. NodeJS 10+

## Development

1. Run `./build.ps1` to build the product
2. Run `migrate-database.ps1 <command> <db-engine>` to prepare database

For example
`.\migrate-database.ps1 recreate sqlserver` - to drop and recreate SQL server database with seeded users
`.\migrate-database.ps1 update postgress` - to run new migrations to the existing postgres database

3a. Run `run.ps1` to start webpack dev server with API host.

3b. Run `build.ps1 -target deploy` to run the whole application in docker (atm postgress is used as a database engine)