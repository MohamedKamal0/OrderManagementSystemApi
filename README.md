Order Management System API
A comprehensive e-commerce order management system built with ASP.NET Core 8.0, following Clean Architecture principles and Domain-Driven Design (DDD) patterns.
🏗️ Architecture
This project implements a layered architecture with clear separation of concerns:

API Layer: RESTful API endpoints with Swagger documentation
Application Layer: Business logic, DTOs, and service implementations
Domain Layer: Core business entities and repository interfaces
Infrastructure Layer: Data access, Entity Framework Core configurations

✨ Features
Core Functionality

Customer Management: Registration, profile updates, and account management
Product Catalog: Category-based product organization with inventory tracking
Shopping Cart: Add, update, and remove items with real-time price calculations
Order Processing: Complete order workflow from creation to fulfillment
Payment Integration: Multiple payment methods including COD and online payments
Address Management: Multiple shipping and billing addresses per customer

Technical Features

Authentication & Authorization: JWT-based security
Caching Strategy: Hybrid caching with Redis and SQL Server
Structured Logging: Serilog integration for comprehensive logging
Data Validation: Comprehensive validation using Data Annotations
AutoMapper: Object-to-object mapping for DTOs
Repository Pattern: Generic repository with Unit of Work
Entity Configurations: Fluent API configurations for all entities

🛠️ Technologies

.NET 8.0
Entity Framework Core 9.0
SQL Server
Redis Cache
Serilog
AutoMapper
JWT Authentication
Swagger/OpenAPI

📦 Project Structure
OrderManagementSystemApi/
├── OrderManagementSystemApi/           # API Layer
│   ├── Controllers/                    # API Controllers
│   ├── Program.cs                      # Application entry point
│   └── appsettings.json               # Configuration
│
├── OrderManagementSystemApplication/   # Application Layer
│   ├── Dtos/                          # Data Transfer Objects
│   ├── Services/                      # Business logic services
│   ├── Mapping/                       # AutoMapper profiles
│   └── Helpers/                       # Log messages & utilities
│
├── OrderManagementSystemDomain/        # Domain Layer
│   ├── Models/                        # Domain entities
│   ├── Enums/                         # Domain enumerations
│   └── Repositories/                  # Repository interfaces
│
└── OrderManagementSystemInfrastructure/ # Infrastructure Layer
    ├── Data/                          # DbContext
    ├── Configurations/                # EF Core configurations
    ├── Repository/                    # Repository implementations
    └── Migrations/                    # Database migrations
🚀 Getting Started
Prerequisites

.NET 8.0 SDK
SQL Server
Redis Server (optional, for caching)

Installation

Clone the repository

bash   git clone https://github.com/yourusername/order-management-system.git
   cd order-management-system

Update connection strings
Edit appsettings.json in the API project:

json   {
     "ConnectionStrings": {
       "DefaultConnstring": "your-sql-server-connection-string",
       "Redis": "localhost:6379"
     }
   }

Apply database migrations

bash   dotnet ef database update --project OrderManagementSystemInfrastructure

Run the application

bash   cd OrderManagementSystemApi
   dotnet run

Access Swagger UI
Navigate to: https://localhost:7018/swagger

📚 API Endpoints
Authentication

POST /api/Auth/register - Register new user
POST /api/Auth/login - User login

Customers

POST /api/Customer/RegisterCustomer - Register customer
GET /api/Customer/GetCustomerById/{id} - Get customer details
PUT /api/Customer/UpdateCustomer - Update customer
DELETE /api/Customer/DeleteCustomer/{id} - Delete customer

Products

POST /api/Product/CreateProduct - Create product (Auth required)
GET /api/Product/GetProductById/{id} - Get product details
GET /api/Product/GetAllProducts - List all products
GET /api/Product/GetAllProductsByCategory/{categoryId} - Products by category
PUT /api/Product/UpdateProductStatus - Update product status (Auth required)
DELETE /api/Product/DeleteProduct/{id} - Delete product (Auth required)

Shopping Cart

GET /api/Shopping/GetCart/{customerId} - Get customer cart (Auth required)
POST /api/Shopping/AddToCart - Add item to cart (Auth required)
DELETE /api/Shopping/RemoveCartItem - Remove cart item (Auth required)
DELETE /api/Shopping/ClearCart - Clear cart (Auth required)

Orders

POST /api/Order/CreateOrder - Create new order (Auth required)
GET /api/Order/GetOrderById/{id} - Get order details
GET /api/Order/GetOrdersByCustomer/{customerId} - Customer orders
PUT /api/Order/UpdateOrderStatus - Update order status (Auth required)

Payments

POST /api/Payment/ProcessPayment - Process payment (Auth required)
GET /api/Payment/GetPaymentById/{paymentId} - Get payment details (Auth required)
GET /api/Payment/GetPaymentByOrderId/{orderId} - Get payment by order (Auth required)
PUT /api/Payment/UpdatePaymentStatus - Update payment status (Auth required)

Categories

POST /api/Category/CreateCategory - Create category
GET /api/Category/GetAllCategories - List all categories

Addresses

POST /api/Address/CreateAddress - Create address
GET /api/Address/GetAddressById/{id} - Get address details
PUT /api/Address/UpdateAddress - Update address
DELETE /api/Address/DeleteAddress - Delete address
GET /api/Address/GetAddressesByCustomer/{customerId} - Customer addresses

🗄️ Database Schema
Core Entities

Customer: Customer information and authentication
Product: Product catalog with pricing and inventory
Category: Product categorization
Cart/CartItem: Shopping cart management
Order/OrderItem: Order processing
Payment: Payment tracking
Address: Customer addresses
Cancellation/Refund: Order cancellation and refund processing

🔐 Security

JWT-based authentication
Password hashing using SHA256
Role-based authorization (User/Admin)
Protected endpoints with [Authorize] attribute

📊 Caching Strategy
The system implements a three-tier caching approach:

L1: In-memory cache (30 seconds expiration)
L2: Redis distributed cache
L3: SQL Server cache (10 minutes expiration)

Used for frequently accessed data like categories and products by category.
🔍 Logging
Structured logging with Serilog:

Console output with detailed formatting
Contextual logging throughout the application
Request/response logging middleware
Custom log messages for business operations

🧪 Best Practices

Clean Architecture: Clear separation of concerns
Repository Pattern: Data access abstraction
Unit of Work: Transaction management
DTOs: Separate data contracts from domain models
Validation: Input validation at multiple layers
Error Handling: Consistent error response format
Async/Await: Asynchronous operations throughout

📝 Configuration
JWT Configuration
json{
  "JWT": {
    "Issuer": "your-issuer",
    "Audience": "your-audience",
    "Lifetime": 30,
    "SigningKey": "your-secret-key-at-least-32-characters"
  }
}
Cache Configuration
Configured in Program.cs:

Redis: Instance name "OrderManagementSystem"
Distributed SQL Server Cache: Table "CacheEntries"
Hybrid Cache: 10 min L2/L3, 30 sec L1

🤝 Contributing
Contributions are welcome! Please follow these steps:

Fork the repository
Create a feature branch (git checkout -b feature/AmazingFeature)
Commit your changes (git commit -m 'Add some AmazingFeature')
Push to the branch (git push origin feature/AmazingFeature)
Open a Pull Request

📄 License
This project is licensed under the MIT License.
👥 Authors

Your Name - Initial work

🙏 Acknowledgments

Clean Architecture pattern by Robert C. Martin
Entity Framework Core documentation
ASP.NET Core best practices
