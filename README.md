# ProductPoc – GraphQL & gRPC

A backend Proof of Concept using **ASP.NET Core (.NET 10)**, **GraphQL (Hot Chocolate)**, and **gRPC**.

## Architecture

```text
Client
  ↓ GraphQL Query
GraphQL API (Port 7002)
  ↓ gRPC Request
gRPC Service (Port 7001)
  ↓
In-Memory Product Data
```

## Features

- Get all products
- Get product by ID
- Filter products by category
- Select specific fields
- Handle missing products and service errors

## Requirements

- .NET 10 SDK
- Visual Studio Code or Visual Studio


```

## Getting Started

**1. Restore and build**

```bash
dotnet restore
dotnet build
```

**2. Trust HTTPS certificate**

```bash
dotnet dev-certs https --trust
```

**3. Run gRPC Service (Terminal 1)**

```bash
dotnet run --project ProductGrpcService --launch-profile https
```

**4. Run GraphQL API (Terminal 2)**

```bash
dotnet run --project ProductGraphQLApi --launch-profile https
```

**5. Open GraphQL IDE**

https://localhost:7002/graphql

## GraphQL Queries & Responses

### 1. Get All Products

```graphql
{
  products {
    id
    name
    description
    price
    category
    stockQuantity
  }
}
```

**Example response (first product):**

```json
{
  "data": {
    "products": [
      {
        "id": 1,
        "name": "Wireless Mouse",
        "description": "Ergonomic Bluetooth mouse",
        "price": 24.99,
        "category": "Electronics",
        "stockQuantity": 45
      }
    ]
  }
}
```

The actual response contains all eight products.

### 2. Field Selection

```graphql
{
  products {
    id
    name
    price
  }
}
```

**Example response (first product):**

```json
{
  "data": {
    "products": [
      {
        "id": 1,
        "name": "Wireless Mouse",
        "price": 24.99
      }
    ]
  }
}
```

The actual response contains all eight products with only the selected fields.

### 3. Get Product by ID

```graphql
{
  productById(id: 1) {
    id
    name
    description
    price
    category
    stockQuantity
  }
}
```

**Response:**

```json
{
  "data": {
    "productById": {
      "id": 1,
      "name": "Wireless Mouse",
      "description": "Ergonomic Bluetooth mouse",
      "price": 24.99,
      "category": "Electronics",
      "stockQuantity": 45
    }
  }
}
```

### 4. Missing Product

```graphql
{
  productById(id: 999) {
    id
    name
    description
    price
    category
    stockQuantity
  }
}
```

**Response:**

```json
{
  "data": {
    "productById": null
  }
}
```

### 5. Filter by Category

```graphql
{
  products(category: "Books") {
    id
    name
    category
  }
}
```

**Response:**

```json
{
  "data": {
    "products": [
      {
        "id": 4,
        "name": "Clean Code",
        "category": "Books"
      },
      {
        "id": 5,
        "name": "The Pragmatic Programmer",
        "category": "Books"
      },
      {
        "id": 6,
        "name": "Learning C#",
        "category": "Books"
      }
    ]
  }
}
```

## Technologies

| Technology | Purpose |
|---|---|
| .NET 10 | Backend framework |
| GraphQL | Client API |
| Hot Chocolate | GraphQL server |
| gRPC | Service communication |
| Protocol Buffers | Service contract |
| In-Memory Data | Sample products |

## Project Scope

This project demonstrates GraphQL-to-gRPC communication using eight sample products.

- No database
- No authentication
- No Docker
- Read-only operations

