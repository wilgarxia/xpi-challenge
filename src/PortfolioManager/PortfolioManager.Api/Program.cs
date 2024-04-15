using PortfolioManager.Api;
using PortfolioManager.Application;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddPresentation(config);
builder.Services.AddApplication(config);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();