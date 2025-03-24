var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
Console.WriteLine("hello");
app.MapGet("/", () => "Hello World!");

app.Run();
