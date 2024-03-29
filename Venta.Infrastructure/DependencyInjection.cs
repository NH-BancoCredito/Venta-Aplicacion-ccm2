﻿using Microsoft.EntityFrameworkCore;
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
using System.Net;
using Polly.Extensions.Http;
using Polly;
using Polly.CircuitBreaker;
using Serilog;
using Confluent.Kafka;
using Venta.Domain.Service.Events;
using Venta.Infrastructure.Services.Events;
using Azure.Core;
using MongoDB.Bson.IO;
using System.Threading;
//using MediatR;

namespace Venta.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfraestructure(
            this IServiceCollection services, IConfiguration configInfo)
        {
            var appConfiguration = new AppConfiguration(configInfo);

            var httpClientBuilder = services.AddHttpClient<IStocksService, StockService>(
                options =>
                {
                    options.BaseAddress = new Uri(appConfiguration.UrlBaseServicioStock);
                    //options.Timeout = TimeSpan.FromMilliseconds(5000);
                }
                ).SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy())
                .AddPolicyHandler(GetBulkHeadPolicy());

            var httpClientBuilder2 = services.AddHttpClient<IPagosService, PagosService>(
                options =>
                {
                    options.BaseAddress = new Uri(appConfiguration.UrlBaseServicioPagos);
                    options.Timeout = TimeSpan.FromMilliseconds(5000);
                }
                ).SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy2())
                .AddPolicyHandler(GetCircuitBreakerPolicy2())
                .AddPolicyHandler(GetBulkHeadPolicy2());

            services.AddDbContext<VentaDbContext>(
                options => options.UseSqlServer(appConfiguration.ConexionDBVentas)
                );

            services.AddRepositories(Assembly.GetExecutingAssembly());

            services.AddLogger(appConfiguration.LogMongoServerDB, appConfiguration.LogMongoDbCollection);

            services.AddProducer(configInfo);
            services.AddEventServices();
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(2,
                            retryAttempts => TimeSpan.FromSeconds(Math.Pow(2, retryAttempts)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            Action<DelegateResult<HttpResponseMessage>, TimeSpan> onBreak = (result, timeSpan) =>
            {
                //Camino altenativo para llamar a otro servicio failure o publicar en una cola(topico kafka)
                Console.WriteLine(result);
            };


            Action onReset = null;


            return HttpPolicyExtensions                
                .HandleTransientHttpError()
                .OrResult(c => !c.IsSuccessStatusCode)
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30),
                onBreak, onReset
                );


        }

        private static IAsyncPolicy<HttpResponseMessage> GetBulkHeadPolicy()
        {
            return Policy.BulkheadAsync<HttpResponseMessage>(1000, int.MaxValue);
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy2()
        {

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(2,
                            retryAttempts => TimeSpan.FromSeconds(Math.Pow(2, retryAttempts)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy2()
        {
            Action<DelegateResult<HttpResponseMessage>, TimeSpan> onBreak = (result, timeSpan) =>
            {
                Console.WriteLine(result);
 
            };


            Action onReset = null;


            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(c => !c.IsSuccessStatusCode)
                .CircuitBreakerAsync(3, TimeSpan.FromMinutes(5),
                //.CircuitBreakerAsync(3, TimeSpan.FromSeconds(30),
                onBreak, onReset
                );


        }

        private static IAsyncPolicy<HttpResponseMessage> GetBulkHeadPolicy2()
        {
            return Policy.BulkheadAsync<HttpResponseMessage>(1000, int.MaxValue);
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

        public static void AddLogger(this IServiceCollection services, string connectionStringDbLog, string collectionName)
        {
            var serilogLogger = new LoggerConfiguration()
                //.MinimumLevel.Error()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.MongoDB(connectionStringDbLog, collectionName: collectionName)
                .CreateLogger();


            services.AddLogging(builder =>
            {
                builder.AddSerilog(logger: serilogLogger, dispose: true);
            });
        }

        private static IServiceCollection AddProducer(this IServiceCollection services, IConfiguration configInfo)
        {
            var appConfiguration = new AppConfiguration(configInfo);


            var config = new ProducerConfig
            {
                Acks = Acks.Leader,
                BootstrapServers = appConfiguration.UrlKafkaServer,
                ClientId = Dns.GetHostName(),
            };

            services.AddSingleton<IPublisherFactory>(sp => new PublisherFactory(config));
            return services;
        }

        private static void AddEventServices(this IServiceCollection services)
        {
            services.AddSingleton<IEventSender, EventSender>();
        }




    }
}
