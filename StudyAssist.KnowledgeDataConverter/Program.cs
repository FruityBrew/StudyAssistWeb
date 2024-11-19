using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StudyAssist.KnowledgeDataConverter;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
var app = builder.Build();
app.Run();
await FromFileToDbConvertor.Convert();
