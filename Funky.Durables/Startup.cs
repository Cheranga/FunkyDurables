using System;
using FluentValidation;
using Funky.Durables;
using Funky.Durables.DataAccess;
using Funky.Durables.DataAccess.CommandHandlers;
using Hatan.Azure.Functions.DependencyInjection.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Funky.Durables
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            RegisterConfigurations(services);
            RegisterValidators(services);
            RegisterDataAccess(services);
        }

        private void RegisterDataAccess(IServiceCollection services)
        {
            services.AddSingleton<IDataStorageFactory, DataStorageFactory>();
            services.AddSingleton<InsertCustomerDataCommandHandler>();
        }

        private void RegisterConfigurations(IServiceCollection services)
        {
            var databaseConfiguration = new DatabaseConfiguration
            {
                ConnectionString = Environment.GetEnvironmentVariable("DatabaseConfiguration.ConnectionString"),
                TableName = Environment.GetEnvironmentVariable("DatabaseConfiguration.TableName")
            };

            services.AddSingleton(databaseConfiguration);
        }

        private void RegisterValidators(IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(Startup).Assembly);
        }
    }
}