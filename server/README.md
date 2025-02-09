# Deployment
Azure: https://integrifyfullstackproject.azurewebsites.net/swagger/index.html

# Team Info
Team 1
Members: Basil Omsha, Aide Barniskyte

# Teamwork

- Designing REST API endpoints
- Database schema
- Workable backend server with ASP.NET Core & Entity Framework

---

## Menu

- [Deployment](#deployment)
- [Team Info](#team-info)
- [Teamwork](#teamwork)
  - [Menu](#menu)
  - [Vision](#vision)
    - [Business Requirements](#business-requirements)
  - [Requirements](#requirements)
  - [Mandatory Features](#mandatory-features)
  - [Optional Features](#optional-features)
  - [Optional Database Advanced Operations](#optional-database-advanced-operations)
  - [Getting Started](#getting-started)
- [Project Setup.  Addtions by Aide and Basil start here ](#project-setup--addtions-by-aide-and-basil-start-here-)
  - [Configuring Connection Strings](#configuring-connection-strings)
    - [Steps to Configure User Secrets](#steps-to-configure-user-secrets)
  - [Entities and Relationships](#entities-and-relationships)
    - [Core Entities](#core-entities)
    - [Supporting Entities](#supporting-entities)
    - [Many-to-Many Linking Entities](#many-to-many-linking-entities)
  - [Database Context](#database-context)
  - [Authentication](#authentication)
    - [GenerateKeyPairs](#generatekeypairs)
      - [**Purpose**](#purpose)
      - [**Key Features**](#key-features)
    - [Why Use Public and Private Keys?](#why-use-public-and-private-keys)
    - [JwtRsaKeysService](#jwtrsakeysservice)
    - [Authentication Flow](#authentication-flow)
    - [Token Generation](#token-generation)
      - [`GenerateAccessToken` Method](#generateaccesstoken-method)
    - [Token Validation](#token-validation)
      - [RegisterAuthentication Method](#registerauthentication-method)
  - [Using Authentication in Swagger](#using-authentication-in-swagger)
    - [1. Obtain Tokens via Login](#1-obtain-tokens-via-login)
    - [2. Authorize Swagger with Access Token](#2-authorize-swagger-with-access-token)
    - [3. Access Protected Endpoints](#3-access-protected-endpoints)
      - [Example: Accessing `GET /api/v1/Users`](#example-accessing-get-apiv1users)
    - [4. Test Token Expiration](#4-test-token-expiration)
    - [5. Refresh Token](#5-refresh-token)
    - [6. Revoke Tokens (Optional)](#6-revoke-tokens-optional)
    - [Effect:](#effect)

---

## Vision

You are required to build a fullstack project management system similar to Trello, Jira, or Monday.

The project can be single- or multi-tenant.

The main requirements are as follows:

- **User Management**
- **Projects and Workspaces**
- **Tasks and Issues**

To take the project to the next level, consider these additional requirements:

- **Collaboration**
- **Real-time Collaboration**
- **Integration with Other Platforms**
- **Reporting and Analytics**

### Business Requirements

- Brainstorm the backend design in terms of entity structure and how they will interact. 
- Discuss the implementation of architecture: CLEAN, DDD, TDD, and any possible pattern your team want to apply (repository pattern, CQRS, etc.).
- **Deadline Management**: Use any project management tool of your choice to ensure timely delivery.

---

## Requirements

_For team assignment, only 1 member should fork the repo, then the admin can invite other members to contribute in the same repo. All members, including the admin, should fork from the common repo, making PRs when changes are needed. Remember to have a develop branch before merging to main. Each feature/schema/bug/issue should have its own branch, and only one member should work on one branch/file at a time. Before making any new branch, make sure to sync the fork and run `git pull` to avoid conflicts with the common team repo._

1. **Create ERD Diagram** with entities, attributes, and relationships. Include the ERD as an image in the project.

2. **Design the API Endpoints** to follow REST API architecture. Document the endpoints in `.md` files, providing detailed explanations of queries, parameters, request bodies, authentication requirements (if applicable), sample responses, and status codes.

3. **Basic Entities** (Additional entities may be included as needed):

   - User
   - Project
   - Workspace
   - Task
   - Issue
   - Comment (optional)
   - Notification (optional)

4. **Develop Backend Server** using CLEAN Architecture:

   - Each collection should contain at least 5 records by the delivery date, except for tasks and issues, which should have at least 20 records each.
   - Implement user authentication and authorization appropriately.
   - Use exception handler to provide meaningful responses to users.
   - Unit testing is required primarily for the Domain and Service layers. It would be ideal to test also the controllers and data access.
   - Deployment: Database [neon.tech](https://neon.tech/) , Server [Azure App Service](https://azure.microsoft.com/en-us/products/app-service) (will be demonstrated in the lecture)
   - The README file should clearly describe the project with sufficient detail and a readable structure.

---

## Mandatory Features

- **User Management**
   - User registration and login functionality
   - User authentication using email/password or other methods (e.g., Google, GitHub)
   - Custom roles and permissions (e.g., HR, Dev, PM, Guest)

- **Projects and Workspaces**
   - Ability to create and manage multiple projects/workspaces
   - Project details: name, description, start/end dates, status

- **Tasks and Issues**
   - Task/issue creation with title, description, priority, and deadline
   - Task/issue tracking: status updates (e.g., To-Do, In Progress, Done)
   - Assign tasks/issues to team members or specific users

- **Boards and Kanban (UI-related)**
   - Customizable boards for different projects/workspaces
   - Card-based representation of tasks/issues on the board
   - Drag-and-drop reordering of cards
   - Board filters and custom views (e.g., due dates, priority)

---

## Optional Features

- **Collaboration and Communication**
   - Notification system: email/text updates on task/issue changes
   - Tagging team members in comments
   - File attachments and commenting on tasks/issues

- **Real-Time Collaboration**
   - Real-time commenting with instant updates for team members
   - Auto-updates for task statuses

- **Integrations and APIs**
   - Integration with Google Drive, Trello, Slack, GitHub issues, calendar, and email clients

- **Gantt Charts and Timelines**
   - Gantt chart visualization for project timelines

- **Reporting and Analytics**
   - Customizable dashboards for project leaders and stakeholders
   - Task/issue analytics: time spent, effort required, conversion rates, etc.

---

## Optional Database Advanced Operations

For the following features, you might take advantage of **transactions**, **complex functions**, and **stored procedures**:

1. **Project and Task Management with Dependencies**
   - Prevent a project from being marked as complete until all tasks and issues are resolved using a transaction.

2. **Bulk Task Assignment or Status Updates**
   - Bulk assign tasks or update statuses within a transaction to ensure either all updates succeed or none do.

5. **Complex Query for Reporting and Analytics**
   - Implement advanced queries for metrics like average completion time, burn-down charts, and task completion rates using user-defined functions or views.

6. **Activity Log Generation**
   - A stored procedure logs user activity whenever a task or comment is modified, capturing timestamps, user IDs, and action descriptions.

7. **Notification System**
   - Use a trigger or stored procedure to automatically generate notifications on task or issue updates.
     
8. **Data Clean-Up and Maintenance Scripts**
    - Use stored procedures for regular cleanup of old data, such as notifications or completed tasks in archived projects.

---

## Getting Started

Here is the recommended order:

- Plan the Database Schema before starting any code.
- Set Up the Project Structure.
- Build the Models.
- Create the Repositories.
- Build the Services.
- Set Up Authentication & Authorization.
- Build the Controllers.
- Implement Error Handling Middleware.
- 
# Project Setup. <span style="color:red;"> Addtions by Aide and Basil start here </span>

## Configuring Connection Strings

For security reasons, it's recommended to use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) to store sensitive information like connection strings.

### Steps to Configure User Secrets

1. **Initialize User Secrets**

   Navigate to the outer most project directory and run:

   ```bash
   dotnet user-secrets init
   ```
   This command adds a UserSecretsId to your .csproj file.

2. **Set Connection Strings**

   Replace the placeholder values with your actual connection details:

   ```bash
   dotnet user-secrets set "ConnectionStrings:localpostgresql" "Host=yourHost;Database=yourDB;Username=Username;Password=yourPSWD"

   dotnet user-secrets set "ConnectionStrings:neonconnection" "Host=yourHost;Database=yourDB;Username=yourUsername;Password=yourPSWD"
   ```

   To switch between the connection head to the `DependencyInjection.cs` file located at `./PMS_Project.Presenter.API/src/`

## Entities and Relationships

The domain model consists of several interconnected entities, each representing core components of the PMS. Below is an overview of the primary entities and their relationships:

### Core Entities

- **User**
  - Represents system users.
  - **Relationships:**
    - **Many-to-Many** with **Workspace** via `Workspace_User`.
    - **Many-to-Many** with **ProjectBoard** via `ProjectBoard_User`.
    - **Many-to-Many** with **TaskCard** via `TaskCard_User`.
  
- **Role**
  - Defines user roles within the system.
  - **Relationships:**
    - **One-to-Many** with **User** `Workspace_User` and `ProjectBoard_User`.
  
- **Workspace**
  - Represents a workspace containing multiple project boards.
  - **Relationships:**
    - **One-to-Many** with **ProjectBoard**.
    - **Many-to-Many** with **User** via `Workspace_User`.
  
- **ProjectBoard**
  - Represents a project board within a workspace.
  - **Relationships:**
    - **Many-to-One** with **Workspace**.
    - **One-to-Many** with **AppList**, and **ProjectBoardLabel**.
    - **Many-to-Many** with **User** via `ProjectBoard_User`.
  
- **AppList**
  - Represents a list within a project board containing multiple tasks.
  - **Relationships:**
    - **Many-to-One** with **ProjectBoard**.
    - **One-to-Many** with **TaskCard**.
  
- **TaskCard**
  - Represents individual tasks within an AppList.
  - **Relationships:**
    - **Many-to-One** with **AppList**, **Priority**, and **Status**.
    - **Many-to-Many** with **User** via `TaskCard_User`.
    - **Many-to-Many** with **ProjectBoardLabel** via `TaskCard_ProjectBoardLabel`.
    - **One-to-Many** with **TaskCardActivity** and **TaskCardChecklist**.

### Supporting Entities

- **Priority & Status**
  - Define the priority levels and statuses for tasks.
  - **Relationships:**
    - **One-to-Many** with **TaskCard**.
  
- **ProjectBoardLabel**
  - Labels associated with project boards for categorizing tasks.
  - **Relationships:**
    - **Many-to-Many** with **TaskCard** via `TaskCard_ProjectBoardLabel`.
  
- **TaskCardActivity**
  - Logs activities related to task cards.
  - **Relationships:**
    - **Many-to-One** with **TaskCard** and **User**.
  
- **TaskCardChecklist & TaskCardChecklistItem**
  - Manage checklists within task cards.
  - **Relationships:**
    - **One-to-Many** between **TaskCardChecklist** and **TaskCardChecklistItem**.
    - **Many-to-One** with **TaskCard**.

### Many-to-Many Linking Entities

- **Workspace_User**
- **ProjectBoard_User**
- **TaskCard_User**
- **TaskCard_ProjectBoardLabel**

## Database Context

The `PostgreSQLDbContext` serves as the bridge between the application and the PostgreSQL database. It defines `DbSet` properties for each entity and configures relationships and table mappings. Since the entities utilize Entity Framework Core annotations and the PostgreSQLDbContext includes additional configuration via the Fluent API, you can map the entities to the database using the following commands from the solution root `PMS_Project`:

  Generate a new migration named InitialCreate based on the current model:
  ```bash
  dotnet ef migrations add InitialCreate --project PMS_Project.Infrastructure --startup-project PMS_Project.Presenter.API
  ```
  Apply the pending migrations to the database, creating the necessary tables and relationships.
  ```bash
  dotnet ef database update --project PMS_Project.Infrastructure --startup-project PMS_Project.Presenter.API
  ```
## Authentication

### GenerateKeyPairs

The `GenerateKeyPairs` class is responsible for generating RSA key pairs and saving them as PEM files. It ensures secure creation and storage of both public and private keys, which are essential for JWT authentication.

**Namespace:** `PMS_Project.Presenter.API.Utils`

#### **Purpose**
- **RSA Key Generation:** Creates a new RSA key pair with a specified key size (default is 4096 bits).
- **PEM File Storage:** Saves the generated public and private keys as PEM-formatted files in a designated directory.

#### **Key Features**
- **Initialization:**
  - **Directory Validation:** Ensures the provided directory path exists; throws an exception if it doesn't.
  - **File Paths:** Defines paths for `id_rsa_pub.pem` (public key) and `id_rsa_priv.pem` (private key).

- **Key Generation and Saving:**
  - **Existence Check:** Skips key generation if PEM files already exist to prevent overwriting.
  - **RSA Creation:** Utilizes `RSA.Create` to generate RSA parameters.
  - **BouncyCastle Conversion:** Converts .NET RSA parameters to BouncyCastle's key format using `DotNetUtilities`.
  - **PEM Writing:** Uses `PemWriter` to write the public and private keys to their respective PEM files.

### Why Use Public and Private Keys?

Using a **public and private key** pair in authentication offers key benefits:

- **Enhanced Security**:
  - **Private Key**: Kept secret on the server, used to **sign** tokens ensuring they are trustworthy.
  - **Public Key**: Shared openly, used by others to **verify** the tokens without accessing the private key.

- **Trustworthiness**:
  - Only the server with the private key can create valid tokens.
  - Anyone with the public key can confirm the token's authenticity.

- **Flexibility**:
  - Multiple services can verify tokens using the public key.
  - Easy to update or rotate keys without disrupting the verification process.

Using public and private keys ensures that your authentication system is secure and that tokens are both reliably issued and validated.

After the keys are generated and for convenience they are saved to user-secrets

### JwtRsaKeysService

The `JwtRsaKeysService` is a critical component responsible for managing RSA key pairs used in the JWT authentication process. It ensures secure generation and validation of JWT tokens by handling the following tasks:

- **Key Loading**:
  - **Private Key**: Retrieves the RSA **private key** from the configuration (`Jwt:Key`) to sign JWT access tokens.
  - **Public Key**: Retrieves the RSA **public key** from the configuration (`Jwt:PublicKey`) to validate incoming JWT tokens.

- **Key Management**:
  - Initializes RSA instances using the loaded PEM-formatted keys.
  - Provides access to the keys through the `SigningKey` (private key) and `ValidationKey` (public key) properties.

- **Security Assurance**:
  - Throws exceptions if either the private or public key is not properly configured, preventing the application from running with incomplete security settings.

### Authentication Flow

1. **Login**: Users authenticate by sending credentials to receive an `AccessToken` and `RefreshToken`.
2. **Access Protected Endpoints**: Use the `AccessToken` to access secured API endpoints.
3. **Token Expiration**: `AccessToken` expires after a set duration (e.g., 60 seconds).
4. **Refresh Token**: Use the `RefreshToken` to obtain a new `AccessToken` without re-authenticating. Saved with User in Db and active for a longer period than access token.
5. **Revoke Token**: Users can revoke their tokens, invalidating the `RefreshToken`.

### Token Generation

Tokens are generated using RSA asymmetric encryption (RS256 algorithm). See above!

#### `GenerateAccessToken` Method

- **Purpose**: Creates a JWT `AccessToken` for authenticated users.
- **Expiration**: Set to expire after 60 seconds for testing.

### Token Validation
Tokens are validated using the public RSA key and the RS256 algorithm.

#### RegisterAuthentication Method
- Purpose: Configures JWT authentication services.
- Configuration:
     - Validates Issuer, Audience, Lifetime, and IssuerSigningKey.
     - Sets ClockSkew to zero to enforce exact expiration.

## Using Authentication in Swagger

### 1. Obtain Tokens via Login

  1. **Navigate to Swagger UI**:  
     [http://localhost:5140/swagger](http://localhost:5140/swagger)

  2. **Access the Authenticate Controller**.

  3. **Use the `POST /api/v1/Authenticate/login` Endpoint**:
     
     - **Request Body**:
       ```json
       {
         "email": "ex@mail.com",
         "password": "Password123!"
       }
       ```
     
  4. **Execute the Request**.

  5. **Copy the `AccessToken` and `RefreshToken` from the Response**.

### 2. Authorize Swagger with Access Token

  1. **Click on the `Authorize` Button** in Swagger UI.

  2. **Enter the Token in the Following Format**: `<AccessToken>`  *(Bearer with sapce are added automatically.)*
   
  3. **Click `Authorize`**, then **`Close`**.

### 3. Access Protected Endpoints

   #### Example: Accessing `GET /api/v1/Users`

   1. **Navigate to the `GET /api/v1/Users` Endpoint**.

   2. **Click on `Try it out`** and then **`Execute`**.

   3. **Receive `200 OK` with the List of Users**.

### 4. Test Token Expiration

  1. **Wait for 60 Seconds** after obtaining the `AccessToken`.

  2. **Attempt to Access Protected Endpoint**:

  - **Expected Response**:  
    `401 Unauthorized` with `error="invalid_token"`, `error_description="The token expired at ..."`

### 5. Refresh Token

  1. **Use the `POST /api/v1/Authenticate/refresh` Endpoint**:

     - **Request Body**:
       ```json
       {
         "accessToken": "<ExpiredAccessToken>",
         "refreshToken": "<RefreshToken>"
       }
       ```

  2. **Execute the Request**.

  3. **Receive New `AccessToken` and `RefreshToken`**. *(Refresh Token is saved automatially into the database)*

  4. **Authorize Swagger with the New Access Token**:

     - **Click on the `Authorize` Button**.

     - **Enter the New Token**:
       ```
       Bearer <NewAccessToken>
       ```

     - **Click `Authorize`**, then **`Close`**.

  5. **Access Protected Endpoints with New Token**:

     - **Navigate to the `GET /api/v1/Users` Endpoint**.

     - **Click on `Try it out`** and then **`Execute`**.

     - **Receive `200 OK` with the List of Users**.

### 6. Revoke Tokens (Optional)

  1. **Use the `DELETE /api/v1/Authenticate/revoke` Endpoint**:

     - **Ensure You're Authorized** with a valid `AccessToken`.

  2. **Execute the Request**.

  3. **Receive `200 OK` Indicating Successful Revocation**.

### Effect:

- **The `RefreshToken` is Invalidated**.
- **Revoke the `AccessToken` by setting the TokenRevoked flag in Db (User Table) to true**.
- **Subsequent Refresh Attempts Will Require Re-authentication**.


