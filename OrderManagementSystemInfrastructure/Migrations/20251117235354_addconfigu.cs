using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementSystemInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addconfigu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Category Name must be between 3 and 100 characters."),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "Description cannot exceed 500 characters."),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "First Name must be between 2 and 50 characters."),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Last Name must be between 2 and 50 characters."),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, comment: "Email is required."),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "PhoneNumber is required."),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "DateOfBirth is required."),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Status name is required.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Product Name must be between 3 and 100 characters."),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Description must be at least 10 characters."),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Price must be between $0.01 and $10,000.00."),
                    StockQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Stock Quantity must be between 0 and 1000."),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DiscountPercentage = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Discount Percentage must be between 0% and 100%."),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false, comment: "Category ID is required.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false, comment: "Customer ID is required."),
                    AddressLine = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Address Line 1 cannot exceed 100 characters."),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "City cannot exceed 50 characters."),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "State cannot exceed 50 characters."),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Postal Code is required.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false, comment: "Customer ID is required."),
                    IsCheckedOut = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indicates whether the cart has been checked out."),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "Timestamp when the cart was created."),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "Timestamp when the cart was last updated.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Order Number cannot exceed 30 characters."),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Order Date is required."),
                    CustomerId = table.Column<int>(type: "int", nullable: false, comment: "Customer ID is required."),
                    BillingAddressId = table.Column<int>(type: "int", nullable: false, comment: "Billing Address ID is required."),
                    ShippingAddressId = table.Column<int>(type: "int", nullable: false, comment: "Shipping Address ID is required."),
                    TotalBaseAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Total Base Amount must be between $0.00 and $100,000.00."),
                    TotalDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Total Discount Amount must be between $0.00 and $100,000.00."),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m, comment: "Shipping Cost must be between $0.00 and $10,000.00."),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Total Amount must be between $0.00 and $110,000.00."),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Invalid Order Status."),
                    CustomerId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses_BillingAddressId",
                        column: x => x.BillingAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses_ShippingAddressId",
                        column: x => x.ShippingAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId1",
                        column: x => x.CustomerId1,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false, comment: "Cart ID is required."),
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "Product ID is required."),
                    Quantity = table.Column<int>(type: "int", nullable: false, comment: "Quantity must be between 1 and 100."),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Unit Price must be between $0.01 and $10,000.00."),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m, comment: "Discount must be between $0.00 and $1,000.00."),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Total Price is calculated based on quantity, unit price, and discount."),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "Timestamp when the cart item was created."),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "Timestamp when the cart item was last updated.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cancellations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false, comment: "Order ID is required."),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "Cancellation reason cannot exceed 500 characters."),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Status of the cancellation request."),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "Date and time when the cancellation was requested."),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Date and time when the cancellation was processed."),
                    ProcessedBy = table.Column<int>(type: "int", nullable: true, comment: "ID of the admin or system that processed the cancellation."),
                    OrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "The order amount at the time of cancellation request initiation."),
                    CancellationCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0.00m, comment: "The cancellation charges applied (if any)."),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Remarks cannot exceed 500 characters.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cancellations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cancellations_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false, comment: "Order ID is required."),
                    OrderId1 = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "Product ID is required."),
                    Quantity = table.Column<int>(type: "int", nullable: false, comment: "Quantity must be between 1 and 100."),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Unit Price must be between $0.00 and $10,000.00."),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m, comment: "Discount must be between $0.00 and $10,000.00."),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Total Price must be between $0.00 and $10,000.00.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId1",
                        column: x => x.OrderId1,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false, comment: "Order ID is required."),
                    OrderId1 = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Payment method (e.g., DebitCard, CreditCard, PayPal, COD)."),
                    TransactionId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Transaction ID from payment gateway."),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Payment amount."),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "Date and time when the payment was made."),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Payment status (Completed, Pending, Failed, Refunded).")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId1",
                        column: x => x.OrderId1,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Refunds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CancellationId = table.Column<int>(type: "int", nullable: false, comment: "Cancellation ID is required."),
                    PaymentId = table.Column<int>(type: "int", nullable: false, comment: "Payment ID is required."),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Refund amount must be between $0.01 and $100,000.00."),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Status of the refund."),
                    RefundMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Method used for refund (e.g., Original Payment Method, Bank Transfer)."),
                    RefundReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Refund Reason cannot exceed 500 characters."),
                    TransactionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Transaction ID cannot exceed 100 characters."),
                    InitiatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()", comment: "Date and time when the refund was initiated."),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Date and time when the refund was completed."),
                    ProcessedBy = table.Column<int>(type: "int", nullable: true, comment: "Track who processed (approved) the refund.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refunds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Refunds_Cancellations_CancellationId",
                        column: x => x.CancellationId,
                        principalTable: "Cancellations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Refunds_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_City_State",
                table: "Addresses",
                columns: new[] { "City", "State" });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CustomerId",
                table: "Addresses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PostalCode",
                table: "Addresses",
                column: "PostalCode");

            migrationBuilder.CreateIndex(
                name: "IX_Cancellations_OrderId",
                table: "Cancellations",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cancellations_ProcessedAt",
                table: "Cancellations",
                column: "ProcessedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Cancellations_RequestedAt",
                table: "Cancellations",
                column: "RequestedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Cancellations_Status",
                table: "Cancellations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Cancellations_Status_RequestedAt",
                table: "Cancellations",
                columns: new[] { "Status", "RequestedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId_ProductId",
                table: "CartItems",
                columns: new[] { "CartId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CreatedAt",
                table: "CartItems",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CreatedAt",
                table: "Carts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CustomerId",
                table: "Carts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CustomerId_IsCheckedOut",
                table: "Carts",
                columns: new[] { "CustomerId", "IsCheckedOut" });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_IsCheckedOut",
                table: "Carts",
                column: "IsCheckedOut");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UpdatedAt",
                table: "Carts",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IsActive",
                table: "Categories",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Name_Unique",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_FirstName_LastName",
                table: "Customers",
                columns: new[] { "FirstName", "LastName" });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_IsActive",
                table: "Customers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PhoneNumber",
                table: "Customers",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Email_Unique",
                table: "Customers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId_ProductId",
                table: "OrderItems",
                columns: new[] { "OrderId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId1",
                table: "OrderItems",
                column: "OrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BillingAddressId",
                table: "Orders",
                column: "BillingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId1",
                table: "Orders",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderDate",
                table: "Orders",
                column: "OrderDate");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingAddressId",
                table: "Orders",
                column: "ShippingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId1",
                table: "Payments",
                column: "OrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentDate",
                table: "Payments",
                column: "PaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethod",
                table: "Payments",
                column: "PaymentMethod");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethod_Status",
                table: "Payments",
                columns: new[] { "PaymentMethod", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Status",
                table: "Payments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Status_PaymentDate",
                table: "Payments",
                columns: new[] { "Status", "PaymentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TransactionId",
                table: "Payments",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId_IsAvailable",
                table: "Products",
                columns: new[] { "CategoryId", "IsAvailable" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsAvailable",
                table: "Products",
                column: "IsAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsAvailable_Price",
                table: "Products",
                columns: new[] { "IsAvailable", "Price" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Price",
                table: "Products",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_CancellationId",
                table: "Refunds",
                column: "CancellationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_CompletedAt",
                table: "Refunds",
                column: "CompletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_InitiatedAt",
                table: "Refunds",
                column: "InitiatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_PaymentId",
                table: "Refunds",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_Status",
                table: "Refunds",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_Status_InitiatedAt",
                table: "Refunds",
                columns: new[] { "Status", "InitiatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_Status_ProcessedBy",
                table: "Refunds",
                columns: new[] { "Status", "ProcessedBy" });

            migrationBuilder.CreateIndex(
                name: "IX_Refunds_TransactionId",
                table: "Refunds",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Status_Name_Unique",
                table: "Statuses",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Refunds");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Cancellations");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
