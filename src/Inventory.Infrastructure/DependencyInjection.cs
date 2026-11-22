using Inventory.Application.Interfaces;
using Inventory.Infrastructure.Commands.Dapper;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Queries.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("InventoryDb")));

            services.AddScoped<
                ICategoryCommandRepository,
                CategoryCommandRepository>();
            services.AddScoped<
                IProductCommandRepository,
                ProductCommandRepository>();
            services.AddScoped<
                IInventoryMovementCommandRepository,
                InventoryMovementCommandRepository>();
            services.AddScoped<
                ICategoryQueryRepository,
                CategoryQueryRepository>();
            services.AddScoped<
                IProductQueryRepository,
                ProductQueryRepository>();
            services.AddScoped<
                IInventoryMovementQueryRepository,
                InventoryMovementQueryRepository>();

            return services;
        }
    }
}