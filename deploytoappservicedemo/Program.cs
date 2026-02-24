// Program.cs
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/status", () =>
    new { status = "running", version = "1.0" });

app.Run();