using DevShop.Application.DTOs;
using DevShop.Application.Implementation;
using DevShop.Application.Interfaces;
using DevShop.Application.Services;
using DevShop.Infrastructure.Persistence;
using DevShop.Infrastructure.Persistence.Repositories;
using DevShop.Infrastructure.Security;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DevShopDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHashingService, PasswordHashingService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUnitOfWork, DevShopDbContext>();

builder.Services.AddControllers()
    .AddFluentValidation(config =>
        config.RegisterValidatorsFromAssembly(typeof(CreateProductDto).Assembly));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();