using Microsoft.EntityFrameworkCore;
using OrderManagementSystemApplication.Maping;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemApplication.Services.Implemntation;
using OrderManagementSystemDomain.Repositories;
using OrderManagementSystemInfrastructure.Data;
using OrderManagementSystemInfrastructure.Repository;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddAutoMapper(typeof(CartMappingProfile));

builder.Services.AddControllers();

//Log.Logger = new LoggerConfiguration()
//.ReadFrom.Configuration(builder.Configuration)
//.Enrich.FromLogContext()
//  .CreateLogger();

//builder.Host.UseSerilog();

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration)
                 .Enrich.FromLogContext();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShoppingRepository, ShoppingRepository>();
builder.Services.AddScoped<IShoppingService, ShoppingService>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICartItemsRepository, CartItemsRepository>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnstring")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
