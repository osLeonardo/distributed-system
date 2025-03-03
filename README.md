# User Client API

This project is a User Client API built with ASP.NET Core and gRPC. It provides endpoints for managing users, locations, products, and product locations. The API uses session-based authentication to track logged-in users.

## Contributors
- Leonardo Spilere
- Natanael Alves

## Features

- User login and logout
- CRUD operations for locations
- CRUD operations for products
- CRUD operations for product locations
- Session-based authentication

## Prerequisites

- .NET 8.0 SDK
- A running instance of the gRPC server

## Getting Started

### Step 1: Clone the Repository

```sh
git clone https://github.com/osLeonardo/distributed-system.git
cd distributed-system
```

### Step 2: Install Dependencies

Restore the required packages:

```sh
dotnet restore
```

### Step 3: Run the gRPC Server

Navigate to the `distributed-system` directory and run the application:

```sh
cd distributed-system
dotnet run
```

### Step 4: Run the User Client API

Navigate to the `user-client` directory and run the application:

```sh
cd user-client
dotnet run
```

### Step 5: Access the API

The API will be available at `https://localhost:5001`. You can use tools like Postman or Swagger UI to interact with the endpoints.

## Endpoints

### User Endpoints

- `POST /login` - Login a user
- `POST /logout` - Logout a user

### Location Endpoints

- `POST /location` - Add a new location
- `PUT /location` - Update an existing location
- `GET /location/{name}` - Get a location by name
- `GET /location/id/{id}` - Get a location by ID
- `GET /locations` - Get all locations
- `DELETE /location` - Delete a location

### Product Endpoints

- `POST /product` - Add a new product
- `PUT /product` - Update an existing product
- `GET /product/{id}` - Get a product by ID
- `GET /products` - Get all products
- `DELETE /product` - Delete a product

### Product Location Endpoints

- `POST /productlocation` - Add a new product location
- `PUT /productlocation` - Update an existing product location
- `GET /productlocation/{id}` - Get a product location by ID
- `GET /productlocations` - Get all product locations
- `DELETE /productlocation` - Delete a product location
