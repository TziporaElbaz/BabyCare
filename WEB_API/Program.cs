var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
//var configuration = new ConfigurationBuilder()
//       .AddJsonFile("appsettings.Development.json")
//       .Build();

//// יצירת WorkerQueueService
//var workerQueueService = new WorkerQueueService(configuration);
