using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
using BabyCare.BL.API;
using BabyCare.DAL.Services;
using BabyCare.BL.Services;
using BabyCare.DAL.API;
using BabyCare.DAL.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<myDatabase>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBabyManagementDAL, BabyManagementDAL>();
builder.Services.AddScoped<IBabyManagementBL, BabyManagementBL>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.MapGet("/", () => "Hello World!"); // להעיר/להסיר

app.MapControllers();

app.Run();