using NewsSync.API.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomSerilog();
builder.Services.AddCustomControllers();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

app.UseCustomMiddlewares();
app.MapControllers();
await app.SeedInitialDataAsync();
app.Run();
