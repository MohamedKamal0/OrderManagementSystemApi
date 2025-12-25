# Order Management System API

A comprehensive e-commerce order management system built with **ASP.NET Core 8.0**, implementing **Clean Architecture**.

## 🏗️ Architecture

This project follows Clean Architecture with four distinct layers:

```
├── OrderManagementSystemDomain/          # Core business entities and interfaces
├── OrderManagementSystemApplication/     # Business logic, DTOs, and services
├── OrderManagementSystemInfrastructure/  # Data access and repository implementations
└── OrderManagementSystemApi/            # RESTful API endpoints and configuration
## 🚀 Features

### Core Functionality
- ✅ **Customer Management**: Registration, profile updates, and soft deletion
- ✅ **Product Catalog**: Category-based organization with stock tracking
- ✅ **Shopping Cart**: Full cart lifecycle with automatic pricing calculations
- ✅ **Order Processing**: Multi-step workflow with validation and inventory management
- ✅ **Payment Gateway**: Support for Credit/Debit cards, PayPal, and Cash on Delivery
- ✅ **Order Tracking**: Real-time status updates with state transition validation
- ✅ **Address Management**: Multiple shipping and billing addresses per customer

### Technical Features
- 🔐 **JWT Authentication**: Secure token-based authentication
- 🛡️ **Permission-Based Authorization**: Fine-grained access control
- 🚀 **Hybrid Caching**: Three-tier caching strategy (L1/L2/L3) with Redis
- ⏱️ **Rate Limiting**: Fixed window limiting (100 requests/minute)
- 📝 **Structured Logging**: Comprehensive logging with Serilog
- ✔️ **Data Validation**: Input validation with Data Annotations
- 🔄 **Transaction Management**: Unit of Work pattern with ACID compliance
- 📊 **Database Indexing**: Optimized queries with strategic indexes
- 🐳 **Docker Support**: Containerized deployment with Docker Compose

## 📋 Prerequisites

- **.NET 8.0 SDK** or later
- **SQL Server 2019+** (or SQL Server 2025 for Docker)
- **Redis Server** (for distributed caching)
- **Docker Desktop** (optional, for containerized deployment)
- **Visual Studio 2022** or **JetBrains Rider** (recommended)

## 🛠️ Installation

### Option 1: Local Development

1. **Clone the repository**
```bash
git clone https://github.com/yourusername/OrderManagementSystemApi.git
cd OrderManagementSystemApi
```

2. **Update connection strings**

Edit `OrderManagementSystemApi/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnstring": "Server=.;Database=OrderManagementDB;Trusted_Connection=True;TrustServerCertificate=true;",
    "Redis": "localhost:6379"
  },
  "JWT": {
    "Issuer": "your-issuer",
    "Audience": "your-audience",
    "Lifetime": 30,
    "SigningKey": "your-256-bit-secret-key-change-this-in-production"
  }
}
```

3. **Install Redis** (if not already installed)
```bash
# Windows (using Chocolatey)
choco install redis-64

# macOS
brew install redis

# Linux
sudo apt-get install redis-server
```

4. **Run database migrations**
```bash
dotnet ef database update --project OrderManagementSystemInfrastructure --startup-project OrderManagementSystemApi
```

5. **Build and run**
```bash
dotnet restore
dotnet build
dotnet run --project OrderManagementSystemApi
```

The API will be available at:
- HTTPS: `https://localhost:7018`
- HTTP: `http://localhost:5295`
- Swagger UI: `https://localhost:7018/swagger`

### Option 2: Docker Deployment

1. **Start the services**
```bash
docker-compose up -d
```

2. **Apply migrations** (first time only)
```bash
docker exec -it ecomers-api dotnet ef database update
```

The API will be available at `http://localhost:8080`

## 📚 API Documentation

Access the interactive Swagger documentation at: `https://localhost:7018/swagger`

### Authentication Flow

1. **Register a user**
```bash
POST /api/Auth/register
{
  "username": "testuser",
  "email": "test@example.com",
  "password": "SecurePassword123!"
}
```

2. **Login to get JWT token**
```bash
POST /api/Auth/login
{
  "email": "test@example.com",
  "password": "SecurePassword123!"
}
```

3. **Use the token** in subsequent requests:
```
Authorization: Bearer <your-jwt-token>
```

### Key Endpoints

#### 👤 Customers
```http
POST   /api/Customer/RegisterCustomer      # Register new customer
GET    /api/Customer/GetCustomerById/{id}  # Get customer details
PUT    /api/Customer/UpdateCustomer        # Update customer info
DELETE /api/Customer/DeleteCustomer/{id}   # Soft delete customer
```

#### 📦 Products
```http
POST   /api/Product/CreateProduct                    # Create product (Write permission)
GET    /api/Product/GetAllProducts                   # List all products
GET    /api/Product/GetProductById/{id}              # Get product details
GET    /api/Product/GetAllProductsByCategory/{catId} # Filter by category
PUT    /api/Product/UpdateProductStatus              # Update availability
DELETE /api/Product/DeleteProduct/{id}               # Delete product (Delete permission)
```

#### 🏷️ Categories
```http
POST   /api/Category/CreateCategory      # Create category (Write permission)
GET    /api/Category/GetAllCategories    # List all categories
```

#### 🛒 Shopping Cart
```http
GET    /api/Shopping/GetCart/{customerId}  # Get customer's cart (Auth required)
POST   /api/Shopping/AddToCart             # Add item to cart (Auth required)
DELETE /api/Shopping/RemoveCartItem        # Remove item (Auth required)
DELETE /api/Shopping/ClearCart             # Clear cart (Auth required)
```

#### 📋 Orders
```http
POST   /api/Order/CreateOrder                   # Create order (Auth required)
GET    /api/Order/GetOrderById/{id}             # Get order details
GET    /api/Order/GetOrdersByCustomer/{custId}  # Get customer orders
PUT    /api/Order/UpdateOrderStatus             # Update status (Auth required)
```

#### 💳 Payments
```http
POST   /api/Payment/ProcessPayment          # Process payment (Auth required)
GET    /api/Payment/GetPaymentById/{id}     # Get payment details (Auth required)
GET    /api/Payment/GetPaymentByOrderId/{id} # Get by order (Auth required)
PUT    /api/Payment/UpdatePaymentStatus     # Update status (Auth required)
```

#### 📍 Addresses
```http
POST   /api/Address/CreateAddress              # Create address
GET    /api/Address/GetAddressById/{id}        # Get address details
PUT    /api/Address/UpdateAddress              # Update address
DELETE /api/Address/DeleteAddress              # Delete address
GET    /api/Address/GetAddressesByCustomer/{id} # Get customer addresses
```

## 🗄️ Database Schema

### Core Entities

| Entity | Description |
|--------|-------------|
| **Customers** | Customer information with soft delete support |
| **Addresses** | Shipping and billing addresses (many-to-one with Customers) |
| **Categories** | Product categorization with active status |
| **Products** | Product catalog with pricing, stock, and discounts |
| **Cart** | Shopping cart header |
| **CartItems** | Individual items in the cart |
| **Orders** | Order header with status and totals |
| **OrderItems** | Line items for each order |
| **Payments** | Payment transactions with status tracking |
| **Cancellations** | Order cancellation requests |
| **Refunds** | Refund processing linked to cancellations |
| **Users** | Authentication and authorization |
| **Userpermission** | Permission assignments |

### Key Relationships

```
Customer 1──╮
            ├──* Address
            ├──* Cart ──* CartItem ──* Product
            └──* Order ──╮
                         ├──* OrderItem ──* Product
                         ├──1 Payment ──1 Refund
                         └──1 Cancellation ──1 Refund
                         
Category 1──* Product
```

### Enums

```csharp
OrderStatus: Pending, Processing, Shipped, Delivered, Canceled
PaymentStatus: Pending, Completed, Failed, Refunded
CancellationStatus: Pending, Approved, Rejected
RefundStatus: Pending, Completed, Failed
Permission: Read, Write, Delete, Update
```

## 🔐 Security

### JWT Configuration

Update these values in production:

```json
{
  "JWT": {
    "Issuer": "your-production-issuer",
    "Audience": "your-production-audience",
    "Lifetime": 30,
    "SigningKey": "CHANGE-THIS-TO-A-STRONG-256-BIT-KEY-IN-PRODUCTION"
  }
}
```

### Permission-Based Authorization

Secure endpoints with custom attributes:

```csharp
[CheckPermission(Permission.Write)]
public async Task<ActionResult> CreateProduct(ProductCreateDto dto)
{
    // Only users with Write permission can access
}
```

### Password Hashing

Passwords are hashed using **PBKDF2** with:
- SHA-512 algorithm
- 16-byte salt
- 10,000 iterations
- 32-byte hash output

## 🎯 Design Patterns

| Pattern | Implementation |
|---------|----------------|
| **Repository** | Data access abstraction for each entity |
| **Unit of Work** | Transaction management across repositories |
| **Dependency Injection** | Constructor injection throughout |
| **DTO Pattern** | Separate request/response objects |
| **AutoMapper** | Automated object-to-object mapping |
| **Response Handler** | Standardized API response structure |
| **Strategy** | Payment method processing |

## 📊 Caching Strategy

Three-tier hybrid caching implementation:

```csharp
L1 (In-Memory)  →  30 seconds TTL
L2 (Redis)      →  10 minutes TTL  
L3 (SQL Server) →  10 minutes TTL
```

**Cached Entities:**
- Categories (all)
- Products (by category)

**Configuration:**
```csharp
builder.Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromMinutes(10),        // L2, L3
        LocalCacheExpiration = TimeSpan.FromSeconds(30) // L1
    };
});
```

## 🚦 Rate Limiting

**Default Configuration:**
- 100 requests per minute per client
- 10 queued requests
- FIFO queue processing
- Applied to read-heavy endpoints

**Custom Configuration:**
```csharp
[EnableRateLimiting("DefaultPolicy")]
public async Task<ActionResult> GetAllProducts()
{
    // Rate limited endpoint
}
```

## 📝 Logging

Structured logging with **Serilog**:

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      }
    ]
  }
}
```

**Log Categories:**
- Request/Response logging
- Error tracking
- Performance monitoring
- Business event logging

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true
```

## 🐳 Docker Support

### Docker Compose Services

```yaml
services:
  - ecomers-api:  ASP.NET Core API (port 8080)
  - sql_server:   SQL Server 2025 (port 1433)
  - (Redis should be added for production)
```

### Commands

```bash
# Build and start
docker-compose up --build -d

# View logs
docker-compose logs -f

# Stop services
docker-compose down

# Remove volumes
docker-compose down -v
##  Author

- **Mohamed Kamal** 
