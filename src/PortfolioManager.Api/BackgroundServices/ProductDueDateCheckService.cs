namespace PortfolioManager.Api.BackgroundServices;

using System.Threading;
using System.Threading.Tasks;

using FluentEmail.Core;

using Microsoft.Extensions.Hosting;

using PortfolioManager.Domain.Products;

public class ProductDueDateCheckService(
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                var fluentEmail = scope.ServiceProvider.GetRequiredService<IFluentEmail>();

                var products = await productRepository.GetProductsCloseToDueDate(stoppingToken);

                foreach (var product in products)
                {
                    await fluentEmail
                        .To(product.ManagerEmail)
                        .Subject("Product Due Date Reminder")
                        .Body($"Your product {product.Description} is close to its due date.")
                        .SendAsync(stoppingToken);
                }
            }
            
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}