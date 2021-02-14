using EthereumQuery;
using EthereumQuery.DataProcesser;
using EthereumQuery.HttpMessageHandlers;
using EthereumQuery.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;

[assembly: FunctionsStartup(typeof(Startup))]
namespace EthereumQuery
{
    internal class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureServices(builder.Services);
        }

        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IEtherHttpMessageHandler, EtherHttpMessageHandler>();
            services.AddScoped<ITransactionsDetailsService, TransactionsDetailsService>();
            services.AddScoped<ITransactionsProcessor, TransactionsProcessor>();
            services.AddSingleton<JsonSerializer>();
            services.AddHttpClient();

            return services;
        }
    }
}
