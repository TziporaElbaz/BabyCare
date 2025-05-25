using Microsoft.AspNetCore.Hosting;
using WEB_API.BL.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(typeof(MappingProfile));
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
//var configuration = new ConfigurationBuilder()
//       .AddJsonFile("appsettings.Development.json")
//       .Build();

//// יצירת WorkerQueueService
//var workerQueueService = new WorkerQueueService(configuration);

