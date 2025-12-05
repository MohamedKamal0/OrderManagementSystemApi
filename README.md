# Order Management System API

A comprehensive e-commerce order management system built with ASP.NET Core 8.0, implementing Clean Architecture principles with Domain-Driven Design (DDD).

## 🏗️ Architecture

This project follows Clean Architecture with four main layers:

- **Domain Layer**: Core business entities and repository interfaces
- **Application Layer**: Business logic, DTOs, services, and AutoMapper profiles
- **Infrastructure Layer**: Data access, EF Core configurations, and repository implementations
- **API Layer**: RESTful endpoints, authentication, rate limiting, and Swagger documentation

## 🚀 Features

### Core Functionality
- **Customer Management**: Registration, profile updates, and soft deletion
- **Product Catalog**: Category-based product organization with availability tracking
- **Shopping Cart**: Add/remove items, quantity management, automatic pricing calculations
- **Order Processing**: Multi-step order creation with address validation and inventory management
- **Payment Gateway**: Multiple payment methods (Credit/Debit Card, PayPal, Cash on Delivery)
- **Order Tracking**: Real-time status updates with state transition validation

### Technical Features
- **Authentication & Authorization**: JWT-based authentication with permission-based access control
- **Caching Strategy**: Hybrid caching (L1/L2/L3) with Redis and SQL Server
- **Rate Limiting**: Fixed window rate limiting (100 requests/minute)
- **Logging**: Structured logging with Serilog
- **Data Validation**: Comprehensive input validation with Data Annotations
- **Transaction Management**: Unit of Work pattern with ACID compliance

## 📋 Prerequisites

- .NET 8.0 SDK
- SQL Server 2019+
- Redis Server (for distributed caching)
- Visual Studio 2022 or JetBrains Rider

## 🛠️ Installation

1. **Clone the repository**
```bash
git clone https://github.com/yourusername/OrderManagementSystemApi.git
cd OrderManagementSystemApi
```

2. **Update connection strings**

Edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnstring": "Server=.;Database=Ordersystemapi;Trusted_Connection=True;TrustServerCertificate=true;",
    "Redis": "localhost:6379"
  }
}
```

3. **Run database migrations**
```bash
dotnet ef database update --project OrderManagementSystemInfrastructure --startup-project OrderManagementSystemApi
```

4. **Build and run**
```bash
dotnet build
dotnet run --project OrderManagementSystemApi
```

The API will be available at `https://localhost:7018` and `http://localhost:5295`

## 📚 API Documentation

Once running, access Swagger UI at: `https://localhost:7018/swagger`

### Key Endpoints

#### Authentication
- `POST /api/Auth/register` - Register new user
- `POST /api/Auth/login` - Login and receive JWT token

#### Customers
- `POST /api/Customer/RegisterCustomer` - Register new customer
- `GET /api/Customer/GetCustomerById/{id}` - Get customer details
- `PUT /api/Customer/UpdateCustomer` - Update customer information
- `DELETE /api/Customer/DeleteCustomer/{id}` - Soft delete customer

#### Products
- `POST /api/Product/CreateProduct` - Create new product (requires Write permission)
- `GET /api/Product/GetAllProducts` - List all products
- `GET /api/Product/GetAllProductsByCategory/{categoryId}` - Filter by category
- `PUT /api/Product/UpdateProductStatus` - Update product availability

#### Shopping Cart
- `GET /api/Shopping/GetCart/{customerId}` - Get customer's cart
- `POST /api/Shopping/AddToCart` - Add item to cart
- `DELETE /api/Shopping/RemoveCartItem` - Remove item from cart
- `DELETE /api/Shopping/ClearCart` - Clear entire cart

#### Orders
- `POST /api/Order/CreateOrder` - Create new order (requires authentication)
- `GET /api/Order/GetOrderById/{id}` - Get order details
- `GET /api/Order/GetOrdersByCustomer/{customerId}` - Get customer orders
- `PUT /api/Order/UpdateOrderStatus` - Update order status

#### Payments
- `POST /api/Payment/ProcessPayment` - Process payment (requires authentication)
- `GET /api/Payment/GetPaymentById/{paymentId}` - Get payment details
- `PUT /api/Payment/UpdatePaymentStatus` - Update payment status

## 🗄️ Database Schema

### Core Entities

**Customers**: Customer information with addresses and order history
**Products**: Product catalog with categories, pricing, and inventory
**Categories**: Product categorization
**Orders**: Order header with status tracking
**OrderItems**: Line items for each order
**Payments**: Payment transactions and status
**Cart/CartItems**: Shopping cart management
**Addresses**: Customer shipping and billing addresses

### Enums

- `OrderStatus`: Pending, Processing, Shipped, Delivered, Canceled
- `PaymentStatus`: Pending, Completed, Failed, Refunded
- `Permission`: Read, Write, Delete, Update

## 🔐 Security

### JWT Configuration
```json
{
  "JWT": {
    "Issuer": "your-issuer",
    "Audience": "your-audience",
    "Lifetime": 30,
    "SigningKey": "your-256-bit-secret-key"
  }
}
```

### Permission-Based Authorization

The system uses custom permission attributes:
```csharp
[CheckPermission(Permission.Write)]
public async Task<ActionResult> CreateProduct(...)
```

## 🎯 Design Patterns

- **Repository Pattern**: Data access abstraction
- **Unit of Work**: Transaction management
- **Dependency Injection**: Loose coupling between layers
- **AutoMapper**: Object-to-object mapping
- **Response Handler**: Standardized API responses
- **Strategy Pattern**: Payment method processing

## 📊 Caching Strategy

The system implements three-tier caching:

- **L1 Cache**: In-memory (30 seconds TTL)
- **L2 Cache**: Redis distributed cache
- **L3 Cache**: SQL Server cache (10 minutes TTL)

Example configuration:
```csharp
builder.Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new HybridCacheEntryOptions
    {
        Expiration = TimeSpan.FromMinutes(10), // L2, L3 
        LocalCacheExpiration = TimeSpan.FromSeconds(30) // L1
    };
});
```

## 🧪 Testing

Run tests with:
```bash
dotnet test
```

## 📝 Logging

Logs are configured with Serilog and written to the console. To customize logging:

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    }
  }
}
```

## 🚦 Rate Limiting

Default rate limiting is configured as:
- 100 requests per minute
- 10 queued requests
- FIFO queue processing


## 👥 Authors

- Mohamed Kamal 

---

**Note**: Remember to update the JWT signing key and connection strings before deploying to production!
