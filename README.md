# Airbnb REST API

This is a REST API built with ASP.NET Core 8.0 and PostgreSQL database.

## Prerequisites

- [Docker](https://www.docker.com/products/docker-desktop/) (with Docker Compose)
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (for local development only)

## Getting Started

### Using Docker (Recommended)

The project is containerized using Docker, which makes it easy to start both the API and the PostgreSQL database with a single command.

1. Clone the repository:
   ```bash
   git clone https://github.com/NadChern/Airbnb-System-Prototype.git
   cd <folder>
   ```

2. Start the application with Docker Compose:
   ```bash
   docker-compose up -d
   ```

   This command will:
    - Build the .NET API from the Dockerfile
    - Start a PostgreSQL container
    - Set up the required network connections
    - Mount the necessary volumes for database persistence
    - Initialize the database with scripts (if any) in the `./db-init/` directory

3. Access the API:
    - The API will be available at http://localhost:5013/swagger/index.html
    - This Swagger UI provides documentation and a testing interface for all available endpoints

4. To stop the application:
   ```bash
   docker-compose down
   ```

   To stop and remove volumes (will delete database data):
   ```bash
   docker-compose down -v
   ```

### Local Development

For development purposes, you may want to run the API directly on your machine:

1. Ensure you have .NET 8.0 SDK installed

2. Update the connection string in `appsettings.Development.json` to point to your PostgreSQL instance

3. Run the API:
   ```bash
   cd backend
   dotnet run
   ```

## Project Structure

- `/backend` - Contains the .NET Core API project
- `/db-init` - Contains SQL scripts that run when the PostgreSQL container starts
- `/backend/Dockerfile` - Instructions for building the API container
- `docker-compose.yml` - Orchestrates the API and database containers

## Database Configuration

The PostgreSQL database uses the following default configuration:
- **Host**: localhost (or `db` from within the Docker network)
- **Port**: 5432
- **Database**: airbnb-database
- **Username**: admin
- **Password**: adminpassword

You can modify these settings in the `docker-compose.yml` file.

## API Documentation

Once the application is running, you can view the full API documentation at:
http://localhost:5013/swagger/index.html

### Authentication & Testing in Postman

This API uses cookie-based authentication. To test the API:

1. **Use Postman**: While Swagger UI is available for viewing the API documentation, you should use [Postman](https://www.postman.com/downloads/) for testing the endpoints since it properly handles cookies.

2. **Authentication Flow**:
    - First, register a new user using the registration endpoint
    - Then, log in with those credentials
    - After successful login, cookies will be automatically stored and used for subsequent requests
    - You can now test all authenticated endpoints

3. **Testing in Postman**:
    - Make sure to enable "Send cookies" in your Postman request settings
    - Use the same Postman instance for all requests to maintain the authentication session

## Troubleshooting

- **API can't connect to database**: Ensure the PostgreSQL container is running with `docker ps`. Check logs with `docker logs postgres-db`
- **Port conflicts**: If port 5013 or 5432 is already in use on your machine, modify the port mappings in `docker-compose.yml`
- **Database initialization issues**: Check the contents of your `./db-init/` directory and ensure SQL scripts are executable and error-free
