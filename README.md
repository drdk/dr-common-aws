# DR.Common.AWS

Simple service layers for AWS methods and addtional LinqPad utilies.# DR Hybrik Client

## Requirements:
* dotnet +3.1.402
* [vault cli](https://www.vaultproject.io/downloads) installed and in PATH env


## How to build:
* `dotnet tool restore`
* * Login to vault if needed:
  * Copy token from https://vault.gcp.dr.dk/ui
  * Run `vault-login.ps1`
  * Paste token into promt
* Copy vault secrets to user secrets:
  * Run `vault-to-usersecrets.ps1`
* `dotnet build`
* `dotnet test`
