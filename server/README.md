# Backend application for task management system

# Table of Contents

- [Backend application for task management system](#backend-application-for-task-management-system)
- [Table of Contents](#table-of-contents)
  - [Introduction](#introduction)
  - [Running the application](#running-the-application)
    - [Environment variables](#environment-variables)
    - [Set up database](#set-up-database)
      - [Generate database schema](#generate-database-schema)
  - [API Documentation](#api-documentation)
    - [Swagger UI](#swagger-ui)
  - [Deployment](#deployment)

## Introduction

This is a backend application for a project management system.
The application is built using ASP.NET Core. Built in collaboration with Aide Barniskyte (aidbar) during the backend module, but further inhanced alone during the front-end module.

## Running the application

To run the application locally

```sh
CD your-project-dir/server/PMS_Project # moves to project root solution

dotnet build # downloads all the necessary packages

#optionally open a new terminal
cd your-project-dir/server/PMS_Project/PMS_Project.Presenter.API

dotnet watch
``` 

### Environment variables

To run the app, configure the required environment variables for this project.

Below are the environment variables required for the application:

| Environment Variable                  | Description                                                      |
| ------------------------------------- | ---------------------------------------------------------------- |
| `Jwt:PublicKey`                       | -----BEGIN PUBLIC KEY-----keycontent----END PUBLIC KEY-----      |
| `Jwt:Key`                             | -----BEGIN RSA PRIVATE KEY-----keycontent----END PUBLIC KEY----- |
| `Jwt:Issuer`                          | yourIssuer                                                       |
| `Jwt:Audience`                        | yourAudience.                                                    |
| `ConnectionStrings:DefaultConnection` | Host=xxx;Database=xxx;Username=xxx;Password=xxx                  |
| `ConnectionStrings:localpostgresql`   | Host=xxx;Database=xxx;Username=xxx;Password=xxx                  |

The RSA PublicKey and Key(privateKey) are auto generated when the application runs. The can be foound in the ``Presenter.API`` layer. These keys are used to generate and decode the JWT tokens. Note these keys are ignored and shouln't be pushed to version control.

Secrets are saved with user-secrets. To set up the secretes run the below (note that you need to in the Presenter.API layer):

 ```bash
   dotnet user-secrets init # initiates user-secrets if not set already

   # set the secrets. Example
   dotnet user-secrets set "ConnectionStrings:localpostgresql" "Host=yourHost;Database=yourDB;Username=Username;Password=yourPSWD"

   ```

### Set up database

You can use a local postgreSQL server or use a cloud one example NeonDb

#### Generate database schema

The schmas are created and push to the DB using Entity framework.

From the Solution root (PMS_Project) run:

Generate a new migration named for example InitialCreate based on the current model:
  ```bash
  dotnet ef migrations add InitialCreate --project PMS_Project.Infrastructure --startup-project PMS_Project.Presenter.API
  ```
  Apply the pending migrations to the database, creating the necessary tables and relationships.
  ```bash
  dotnet ef database update --project PMS_Project.Infrastructure --startup-project PMS_Project.Presenter.API
  ```
  Whenever there is a change to any of the schemas, create a new migration with a unique name and then update the databse as in the scripts above.

## API Documentation

### Swagger UI

After running the application, the API documentation can be accessed
from following url: [http://localhost:5140/swagger-ui/index.html](http://localhost:5140/swagger-ui/index.html)

The APIs can then called from the front-end as follow:

- [http://localhost:5140/v1/api/specifcendpoint](http://localhost:5140/v1/api/specifcendpoint) #see docs

## Deployment

Application is deployed on Azure. You can access it with
[https://integrifyfullstackprojectbasel-f6ead6bagma7hch5.swedencentral-01.azurewebsites.net/](https://integrifyfullstackprojectbasel-f6ead6bagma7hch5.swedencentral-01.azurewebsites.net/).

API documentation available at [https://integrifyfullstackprojectbasel-f6ead6bagma7hch5.swedencentral-01.azurewebsites.net/swagger/index.html](https://integrifyfullstackprojectbasel-f6ead6bagma7hch5.swedencentral-01.azurewebsites.net/swagger/index.html).

Deployment is automated from the main branch using GitHubActions.
