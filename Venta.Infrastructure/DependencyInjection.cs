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
using Venta.Infrastructure.Repositories;
using Venta.Infrastructure.Repositories.Base;

namespace Venta.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfraestructure(
            this IServiceCollection services, string connectionString
            )
        {

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

        private static void SetHttpClient<TClient, TImplementation>(this IServiceCollection services, string constante) where TClient : class where TImplementation : class, TClient
        {

            services.AddHttpClient<TClient, TImplementation>(options =>
            {
                options.Timeout = TimeSpan.FromMilliseconds(2000);
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(30))
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                //if (EnvironmentVariableProvider.IsDevelopment())
                //{
                //    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                //}

                return handler;
            });
        }

    }
}
