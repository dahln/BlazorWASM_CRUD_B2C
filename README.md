[![API CI/CD](https://github.com/dahln/BlazorWASM_CRUD_B2C/actions/workflows/master_blazorwasm-crud-b2c-api.yml/badge.svg)](https://github.com/dahln/BlazorWASM_CRUD_B2C/actions/workflows/master_blazorwasm-crud-b2c-api.yml) [![Web Apps CI/CD](https://github.com/dahln/BlazorWASM_CRUD_B2C/actions/workflows/azure-static-web-apps-salmon-bush-017f78810.yml/badge.svg)](https://github.com/dahln/BlazorWASM_CRUD_B2C/actions/workflows/azure-static-web-apps-salmon-bush-017f78810.yml)


# BlazorWASM_CRUD_B2C
This project is a demo of how to build a .NET web application, using Blazor WASM which consumes an API. The client app (blazor), is separate from the API app. Cors is used to authorize the client. Both applications use Azure B2C for authentication/authorization. On the initial startup of the application, be sure the DB is correctly created. You will need MSSQL Server installed (I use MSSQL Developer Edition). Assuming the connection string is correct in appSettings.json, the application should create the DB on startup.

### Setup:
Clone the repo. To run the application, you need to run both the API and the Blazor app. If you have a cors issue, ensure the port used in Startup.cs is the same as the port the client app is running on.

The Azure B2C settings are set in the appSettings.json files. This public repo does not include those values - set those values using your specific Azure B2C values/id's.

### DB Migration Commands:
Add-Migration -Project BlazorWASM_CRUD_B2C.Data -StartupProject BlazorWASM_CRUD_B2C.API -Name InitialCreate

Update-Database -Project BlazorWASM_CRUD_B2C.Data -StartupProject BlazorWASM_CRUD_B2C.API

### Ignore Local Changes to AppSettings.json:
Sensative configuration data, such as the DB connection strings, or Azure B2C config to the appsettings.json files. AppSettings existin the API and Web projects. Do not check in these values to the repo. Use the following commands to ignore changes to the appsettings.json files:
 - git update-index --assume-unchanged .\BlazorWASM_CRUD_B2C.API\appsettings.json            
 - git update-index --assume-unchanged .\BlazorWASM_CRUD_B2C.Web\wwwroot\appsettings.json
 - use '--no-assume-unchanged' to reverse the ignore.
