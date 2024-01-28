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
using Venta.CrossCutting.Configs;
using Microsoft.Extensions.Configuration;

namespace Venta.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfraestructure(
            this IServiceCollection services, IConfiguration configInfo
            )
        {
            var appConfiguration = new AppConfiguration(configInfo);

            var httpClientBuilder =  services.AddHttpClient<IStocksService, StockService>(
                options=>
                {
                    options.BaseAddress = new Uri(appConfiguration.UrlBaseServicioStock);
                    options.Timeout = TimeSpan.FromMilliseconds(2000);
                }
                );           

            services.AddDbContext<VentaDbContext>(
                options=>options.UseSqlServer(appConfiguration.ConexionDBVentas)
                );

            services.AddRepositories(Assembly.GetExecutingAssembly());
        }
       

        public static void
                AddRepositories(this IServiceCollection services, Assembly assembly)

        {


            var respositoryTypes = assembly
                .GetExportedTypes().Where(item => item.GetInterface(nameof(IRepository)) != null).ToList();


            foreach (var repositoryType in respositoryTypes)
            {
                var repositoryInterfaceType = repositoryType.GetInterfaces()
                    .Where(item => item.GetInterface(nameof(IRepository)) != null)
                    .First();

                services.AddScoped(repositoryInterfaceType, repositoryType);
            }
        }



    }
}
