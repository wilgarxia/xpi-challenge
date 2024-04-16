using PortfolioManager.Api.Extensions;
using PortfolioManager.Application.Extensions;
using PortfolioManager.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddControllers();

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