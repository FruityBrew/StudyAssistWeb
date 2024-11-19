var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
var app = builder.Build();
app.MapControllers();
app.Run();
