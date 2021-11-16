# BlazorWASM_CRUD_B2C
This project is a demo of how to build a .NET web application, using Blazor WASM which consumes an API. The client app (blazor), is separate from the API app. Cors is used to authorize the client. Both applications use Azure B2C for authentication/authorization. This demo uses Sqlite as the DB. Code to use MSSQL is commented out, but can easily re-enabled - just be sure to delete and recreate new DB migrations.

### Setup:
Clone the repo. To run the application, you need to run both the API and the Blazor app. If you have a cors issue, ensure the port used in Startup.cs is the same as the port the client app is running on.

### DB Migration Commands:
Add-Migration -Project BlazorWASM_CRUD_B2C.Data -StartupProject BlazorWASM_CRUD_B2C.API -Name InitialCreate

Update-Database -Project BlazorWASM_CRUD_B2C.Data -StartupProject BlazorWASM_CRUD_B2C.API

### Ignore Local Changes to AppSettings.json:
Sensative configuration data, such as the DB connection strings, or Azure B2C config to the appsettings.json files. AppSettings existin the API and Web projects. Do not check in these values to the repo. Use the following commands to ignore changes to the appsettings.json files:
 - git update-index --assume-unchanged .\BlazorWASM_CRUD_B2C.API\appsettings.Development.json
 - git update-index --assume-unchanged .\BlazorWASM_CRUD_B2C.API\appsettings.json            
 - git update-index --assume-unchanged .\BlazorWASM_CRUD_B2C.Web\wwwroot\appsettings.json
