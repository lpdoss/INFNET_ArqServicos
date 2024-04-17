using Microsoft.EntityFrameworkCore;
using PaymentService.Repositories;
using PaymentService.Services;
using Shared.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);
var builderConfig = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper
var assembly = System.Reflection.Assembly.GetExecutingAssembly();
builder.Services.AddAutoMapper(assembly);
// DB
builder.Services.AddDbContext<PaymentDbContext>(c => c.UseSqlServer(builderConfig["PaymentDb"]));
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
// Project Services
builder.Services.AddScoped<IOrderService, OrderService>();
// RabbitMQ
builder.Services.AddSingleton<IEventPublisher, EventPublisher>();
builder.Services.AddHostedService<RabbitMQConsumer_ConfirmedStockEvent>();
builder.Services.AddHostedService<RabbitMQConsumer_NoStockEvent>();

var app = builder.Build();

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
