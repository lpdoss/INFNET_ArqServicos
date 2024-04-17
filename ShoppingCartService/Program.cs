using Microsoft.EntityFrameworkCore;
using ShoppingCartService.Repositories;
using ShoppingCartService.Services;

var builder = WebApplication.CreateBuilder(args);
var builderConfig = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var assembly = System.Reflection.Assembly.GetExecutingAssembly();
builder.Services.AddAutoMapper(assembly);
// DB Config
builder.Services.AddDbContext<ShoppingCartDbContext>(c => c.UseSqlServer(builderConfig["ShoppingCartDb"]));
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddHostedService<RabbitMQConsumer_DeleteCartEvent>();

var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
