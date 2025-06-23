using AutoMapper;
using BL.API;
using BL.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Cors.Infrastructure; 

using WEB_API.BL.API;
using WEB_API.BL.Services;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;
using WEB_API.DAL.Services;

var projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.Parent.FullName;
var dalFolder = Path.Combine(projectRoot, "DAL");
AppDomain.CurrentDomain.SetData("DataDirectory", dalFolder);
Console.WriteLine("Data directory:", AppDomain.CurrentDomain.GetData("DataDirectory"));

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddDbContext<myDatabase>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBabyManagementDAL, BabyManagementDAL>();
builder.Services.AddScoped<IBabyManagementBL, BabyManagementBL>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddScoped<IWorkersManagmentDAL, WorkersManagementDAL>();
builder.Services.AddScoped<IWorkerManegmentBL, WorkerManegmentBL>();

builder.Services.AddScoped<IMapper, Mapper>(); 

var app = builder.Build();


app.UseCors("AllowReactApp"); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
