using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Venta.Domain.Repositories;
using Venta.Domain.Services.WebServices;
using Venta.Infrastructure.Repositories;
using Venta.Infrastructure.Repositories.Base;
using Venta.Infrastructure.Services.WebServices;

namespace Venta.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfraestructure(
            this IServiceCollection services, string connectionString
            )
        {

            var httpClientBuilder =  services.AddHttpClient<IStocksService, StockService>(
                options=>
                {
                    options.BaseAddress = new Uri("https://localhost:7065/");
                    options.Timeout = TimeSpan.FromMilliseconds(2000);
                }
                );
          

            services.AddDbContext<VentaDbContext>(
                options=>options.UseSqlServer(connectionString)
                );

            services.AddRepositories();
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IProductoRepository, ProductoRepository>();

            services.AddScoped<IVentaRepository, VentaRepository>();
        }

        

    }
}
