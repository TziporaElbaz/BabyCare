using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
using WEB_API.BL.API;
using WEB_API.DAL.Services;
using WEB_API.BL.Services;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;
using Microsoft.OpenApi.Models;

using AutoMapper;
using WEB_API.Services;
using WEB_API.BL.API;




var builder = WebApplication.CreateBuilder(args);
var projectRoot =
Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.Parent.FullName;

// Now join just "DAL" (not "DAL", "Data")
var dalFolder = Path.Combine(projectRoot, "DAL");
AppDomain.CurrentDomain.SetData("DataDirectory",dalFolder);

// הוספת שירותים
builder.Services.AddControllers();
IServiceCollection serviceCollection=builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<myDatabase>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBabyManagementDAL, BabyManagementDAL>();
builder.Services.AddScoped<IBabyManagementBL, BabyManagementBL>();
builder.Services.AddScoped<IWorkersManagmentDAL, WorkersManagementDAL>();
builder.Services.AddScoped<IWorkerManegmentBL, WorkerManegmentBL>();
builder.Services.AddScoped<IVaccineManagementDAL, VaccineManagementDAL>();
builder.Services.AddScoped<IVaccineManagementBL, VaccineManagementBL>();
builder.Services.AddScoped<IBabyVaccineManagementDAL, BabyVaccineManagementDAL>();
builder.Services.AddScoped<IBabyVaccineManagementBL, BabyVaccineManagementBL>();
builder.Services.AddScoped<IMapper, Mapper>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

// הגדרות אפליקציה
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

// שימוש במדיניות CORS
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

//app.MapControllers();
app.Run();
