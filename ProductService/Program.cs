using Microsoft.EntityFrameworkCore;
using ProductService.Repositories;
using ProductService.Services;

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
builder.Services.AddDbContext<ProductDbContext>(c => c.UseSqlServer(builderConfig["ProductDb"]));
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IProductService, ProductService.Services.ProductService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

app.Run();
