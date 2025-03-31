# Airbnb-System-Prototype

## Overview
This is a **full-stack prototype** of an Airbnb-inspired system, developed as part of the *CPSC 5200 Software Architecture and Design* course. The project was built over a two-week sprint and demonstrates key architectural concepts such as:

- RESTful API with **authentication** and **role-based access control** (`rest` branch)
- Backend built using **.NET (C#)**
- Frontend using **Razor Pages**
- **PostgreSQL** as the database
- Containerized with **Docker Compose**

> ⚠️ *Note: This is a short-term prototype. Some features such as login-required property creation are not fully implemented due to time constraints.*

Prerequisites
Before running the project, ensure you have the following installed:
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)
- [PostgreSQL](https://www.postgresql.org/download/) (if running the database locally instead of Docker)
 
Getting Started
 
Clone the Repository
 
Running the Project
 
Step 1: Open Docker and Start Containers
Make sure Docker is running, then execute:
 
docker-compose up -d
 
This will start all required containers, including PostgreSQL.
 
Step 2: Start Backend Server
1. Open a terminal and navigate to the backend directory:
       cd backend
2. Run the backend service:
    dotnet build ,
    dotnet run

 
Step 3: Start Frontend Application
1. Open another terminal and navigate to the frontend directory:
    cd frontend
2. Run the Blazor frontend application:
    dotnet build,
    dotnet run
3. Click on the link for the localhost to open the project
 
Note: For the prototype, authentication such as required login to create property is not included while implementing due to time constraint.
 
API Documentation
The API implementation is in the `rest` branch and includes:
- Swagger documentation for easy API exploration
- Role-based authentication and login requirement functionalities
 
To access Swagger UI:
 
http://localhost:5013/swagger/index.html
 
 
Stopping the Services
To stop all running containers:
 
docker-compose down
