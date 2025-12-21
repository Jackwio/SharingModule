# SharingModule API Documentation

## API Specification

This repository includes OpenAPI 3.0 specification files that describe all available API endpoints for the SharingModule application:

- **JSON format:** [`openapi.json`](openapi.json)
- **YAML format:** [`openapi.yaml`](openapi.yaml)

Both files contain the same specification in different formats. Use whichever format you prefer for your tooling.

## Base URL

The application runs on the following base URL by default:

**Development Environment:**
```
https://localhost:44369
```

You can configure the base URL in the `appsettings.json` file located at:
```
src/SharingModule.HttpApi.Host/appsettings.json
```

Look for the `App:SelfUrl` setting:
```json
{
  "App": {
    "SelfUrl": "https://localhost:44369"
  }
}
```

## API Endpoints

The SharingModule API provides the following endpoints for managing share links:

### ShareLink Management

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/share-links/{id}` | Get a specific share link by ID |
| GET | `/api/share-links` | Get a paginated list of share links |
| POST | `/api/share-links` | Create a new share link |
| PUT | `/api/share-links/{id}` | Update an existing share link |
| DELETE | `/api/share-links/{id}` | Delete a share link |
| POST | `/api/share-links/{id}/revoke` | Revoke a share link |
| POST | `/api/share-links/validate` | Validate a share link token and record access |
| GET | `/api/share-links/by-resource` | Get share links for a specific resource |

## Using the API Specification

### With Swagger UI

When you run the application, you can access the interactive Swagger UI at:
```
https://localhost:44369/swagger
```

### Importing into API Development Tools

You can import the `openapi.json` file into popular API development tools:

#### Postman
1. Open Postman
2. Click "Import" button
3. Select the `openapi.json` or `openapi.yaml` file
4. Postman will create a collection with all endpoints

#### Insomnia
1. Open Insomnia
2. Go to Application → Preferences → Data → Import Data
3. Select the `openapi.json` file

#### VS Code with REST Client
Use the OpenAPI specification with extensions like "REST Client" or "Thunder Client"

### Code Generation

You can use the OpenAPI specification to generate client code in various languages:

#### Using OpenAPI Generator CLI
```bash
# Install OpenAPI Generator
npm install @openapitools/openapi-generator-cli -g

# Generate TypeScript client
openapi-generator-cli generate -i openapi.json -g typescript-axios -o ./client-typescript

# Generate C# client
openapi-generator-cli generate -i openapi.json -g csharp-netcore -o ./client-csharp

# Generate Java client
openapi-generator-cli generate -i openapi.json -g java -o ./client-java
```

#### Using NSwag
```bash
# Install NSwag
dotnet tool install -g NSwag.ConsoleCore

# Generate C# client
nswag openapi2csclient /input:openapi.json /output:ShareLinkClient.cs
```

## Authentication

The API uses OAuth 2.0 authentication with OpenIddict. To access protected endpoints:

1. Obtain an access token from the authorization server
2. Include the token in the `Authorization` header:
   ```
   Authorization: Bearer {your-access-token}
   ```

### OAuth 2.0 Endpoints

- **Authorization URL:** `https://localhost:44369/connect/authorize`
- **Token URL:** `https://localhost:44369/connect/token`
- **Scopes:** `SharingModule`

For Swagger UI testing, use the client ID configured in `appsettings.json`:
```json
{
  "AuthServer": {
    "SwaggerClientId": "SharingModule_Swagger"
  }
}
```

## Resource Types

The API supports sharing the following resource types:

- **Page** (0) - Page resource
- **TaskBoard** (1) - Task board resource
- **Workspace** (2) - Workspace resource

## Share Link Types

Share links can have the following types:

- **Private** (0) - Resource not shared
- **SingleUse** (1) - Single use share link
- **MultipleUse** (2) - Multiple use share link

## Example Requests

### Create a Share Link

```bash
POST https://localhost:44369/api/share-links
Content-Type: application/json
Authorization: Bearer {token}

{
  "resourceType": "Page",
  "resourceId": "my-page-123",
  "linkType": "MultipleUse",
  "isReadOnly": true,
  "allowComments": false,
  "allowAnonymous": true,
  "expiresAt": "2025-12-31T23:59:59Z"
}
```

### Get Share Links

```bash
GET https://localhost:44369/api/share-links?ResourceType=Page&SkipCount=0&MaxResultCount=10
Authorization: Bearer {token}
```

### Validate a Share Link

```bash
POST https://localhost:44369/api/share-links/validate
Content-Type: application/json

{
  "token": "share-link-token-here",
  "isAnonymous": true,
  "ipAddress": "192.168.1.1",
  "userAgent": "Mozilla/5.0..."
}
```

## Running the Application

To start the application and access the API:

1. Ensure you have .NET 10.0+ SDK installed
2. Set up the database (PostgreSQL by default)
3. Run database migrations:
   ```bash
   cd src/SharingModule.DbMigrator
   dotnet run
   ```
4. Start the API host:
   ```bash
   cd src/SharingModule.HttpApi.Host
   dotnet run
   ```
5. Access the API at `https://localhost:44369`
6. View interactive documentation at `https://localhost:44369/swagger`

## Support

For more information about ABP Framework and the application architecture, see:
- [ABP Framework Documentation](https://abp.io/docs)
- [Web Application Development Tutorial](https://abp.io/docs/latest/tutorials/book-store/part-01)
- [Application Startup Template Structure](https://abp.io/docs/latest/solution-templates/layered-web-application)
