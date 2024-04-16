using PortfolioManager.Api;
using PortfolioManager.Application;
using PortfolioManager.Infrastructure;
using PortfolioManager.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services
    .AddPresentation(config)
    .AddApplication()
    .AddInfrastructure(config);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.ApplyMigrations();

app.Run();