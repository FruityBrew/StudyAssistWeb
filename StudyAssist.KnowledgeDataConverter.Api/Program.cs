using StudyAssist.KnowledgeDataConverter.Api;
using Utilities.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IKnowledgeDataConverter, FromFileToDbConvertor>();
builder.Services.AddScoped<IKnowledgeDataProvider, KnowledgeDataProvider>();
var app = builder.Build();
app.MapControllers();
app.Run();
