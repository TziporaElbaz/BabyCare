using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WEB_API.BL.API;
using WEB_API.BL.Models;
using WEB_API.BL.Services;
using WEB_API.DAL.API;
using WEB_API.DAL.Models;
using WEB_API.DAL.Services;
using WEB_API.Services;

//make sure database is in DAL
var projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.Parent.FullName;
var dalFolder = Path.Combine(projectRoot, "DAL");
AppDomain.CurrentDomain.SetData("DataDirectory", dalFolder);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowCredentials()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddDbContext<myDatabase>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBabyManagementDAL, BabyManagementDAL>();
builder.Services.AddScoped<IBabyManagementBL, BabyManagementBL>();
builder.Services.AddScoped<IAvailableAppointmentManagementDAL, AvailableAppointmentManagementDAL>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IBabyVaccineManagementDAL, BabyVaccineManagementDAL>();
builder.Services.AddScoped<IAppointmentManagementDAL, AppointmentManagementDAL>();
builder.Services.AddScoped<IAppointmentsManagementBL, AppointmentsManagementBL>();
builder.Services.AddScoped<IVaccineManagementBL, VaccineManagementBL>();
builder.Services.AddScoped<IWorkersManagmentDAL, WorkersManagementDAL>();
builder.Services.AddScoped<IWorkerManegmentBL, WorkerManegmentBL>();
builder.Services.AddScoped<IWorkerShiftManagementDAL, WorkerShiftManagementDAL>();
builder.Services.AddScoped<IShiftManagementDAL, ShiftManagementDAL>();
builder.Services.AddScoped<IAvailableAppointmentsManagementBL, AvailableAppointmentsManagementBL>();
builder.Services.AddScoped<IVaccineManagementDAL, VaccineManagementDAL>();
builder.Services.AddScoped<IBabyVaccineManagementDAL, BabyVaccineManagementDAL>();
builder.Services.AddScoped<IBabyVaccineManagementBL, BabyVaccineManagementBL>();
builder.Services.AddScoped<IMapper, Mapper>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddHttpClient();
//builder.Services.AddHostedService<AppointmentBackgroundService>();

//Add jwt secret key if empty
if (string.IsNullOrEmpty(builder.Configuration["JwtSettings:Key"]))
{
    var generatedKey = JwtService.GenerateSecretKey();
    builder.Configuration["JwtSettings:Key"] = generatedKey;
}

// JWT Configuration
builder.Services.Configure<JwtSettings>(options =>
{
    options.Issuer = builder.Configuration["JwtSettings:Issuer"];
    options.Audience = builder.Configuration["JwtSettings:Audience"];
    options.Key = builder.Configuration["JwtSettings:Key"];
});

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var secretKey = builder.Configuration["JwtSettings:Key"];
    var key = Encoding.UTF8.GetBytes(secretKey);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"]
    };
});

var app = builder.Build();

app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
