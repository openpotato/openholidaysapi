![GitHub](https://img.shields.io/github/license/openpotato/openholidaysapi)

# OpenHolidays API

The service behind the OpenHolidays API. Build with [.NET 7](https://dotnet.microsoft.com/).

## Technology stack

+ [PostgreSQL 14](https://www.postgresql.org/) as database
+ [ASP.NET 7](https://dotnet.microsoft.com/apps/aspnet) as web framework
+ [Entity Framework Core 7](https://docs.microsoft.com/ef/) as ORM layer
+ [Swagger UI](https://swagger.io/tools/swagger-ui/) for OpenAPI based documentation

## Getting started 

The following instructions show you how to set up a development environment on your computer.

### Prerequisites

+ Set up a local PosgreSQL 14 (or higher) instance.
+ Clone or download the repository [OpenHolidaysApi.Data](https://github.com/openpotato/openholidaysapi.data).
+ Clone or download this repository.
+ Open the solution file `OpenHolidaysApi.sln` in Visual Studio 2022.

### Configure the OpenHolidaysApi CLI

+ Switch to the project `OpenHolidaysApi.CLI`.
+ Make a copy of the the `appsettings.json` file and name it `appsettings.Development.json`.
+ Exchange the content with the following JSON document and adjust the values to your needs. This configures the root folder for the csv data sources (the `src` folder in your local [OpenHolidaysApi.Data](https://github.com/openpotato/openholidaysapi.data) repository) and the database connection.
  
  ``` json
  "Sources": {
    "RootFolderName": "c:\\openholidaysapi.data\\src"
  },
  "Database": {
    "Server": "localhost",
    "Database": "OpenHolidaysApi",
    "Username": "postgres",
    "Password": "qwertz"
  }
  ```

### Create and populate the database

+ Build the `OpenHolidaysApi.CLI` project. 
+ Run the `OpenHolidaysApi.CLI` project with parameter `initdb -p`. This will create and populate the PostgreSQL database.

### Configure the OpenHolidaysApi WebService

+ Switch to the  `OpenHolidaysApi.WebService`. 
+ Make a copy of the the `appsettings.json` file and name it `appsettings.Development.json`.
+ Exchange the content with the following JSON document and adjust the values to your needs. This configures the database connection.

  ``` json
  "Database": {
    "Server": "localhost",
    "Database": "OpenHolidaysApi",
    "Username": "postgres",
    "Password": "qwertz"
  }
  ```

### Build and test the API

+ Build the `OpenHolidaysApi.WebService` project.
+ Run the `OpenHolidaysApi.WebService` project and play with the Swagger UI.

## Can I help?

Yes, that would be much appreciated. The best way to help is to post a response via the Issue Tracker and/or submit a Pull Request.
