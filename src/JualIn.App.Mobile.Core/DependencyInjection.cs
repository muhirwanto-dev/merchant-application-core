using JualIn.App.Mobile.Core.Configurations;
using JualIn.App.Mobile.Core.Infrastructure.Api.Abstractions;
using JualIn.App.Mobile.Core.Modules.Auth.Abstractions;
using JualIn.App.Mobile.Core.Modules.Auth.Services;
using JualIn.App.Mobile.Core.Modules.Catalogs.Abstractions;
using JualIn.App.Mobile.Core.Modules.Catalogs.Persistence;
using JualIn.App.Mobile.Core.Modules.Catalogs.Services;
using JualIn.App.Mobile.Core.Modules.Inventories.Abstractions;
using JualIn.App.Mobile.Core.Modules.Inventories.Factories;
using JualIn.App.Mobile.Core.Modules.Inventories.Models;
using JualIn.App.Mobile.Core.Modules.Inventories.Persistence;
using JualIn.App.Mobile.Core.Modules.Inventories.Services;
using JualIn.App.Mobile.Core.Modules.Payments.Abstractions;
using JualIn.App.Mobile.Core.Modules.Payments.Factories;
using JualIn.App.Mobile.Core.Modules.Payments.Models;
using JualIn.App.Mobile.Core.Modules.Payments.Services;
using JualIn.App.Mobile.Core.Modules.Sales.Abstractions;
using JualIn.App.Mobile.Core.Modules.Sales.Factories;
using JualIn.App.Mobile.Core.Modules.Sales.Persistence;
using JualIn.Domain.Catalogs.Entities;
using JualIn.Domain.Common.Messaging;
using JualIn.Domain.Inventories.Entities;
using JualIn.Domain.Sales.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using SingleScope.Persistence;
using JualIn.App.Mobile.Core.Infrastructure.Persistence.Abstractions;
using SingleScope.Persistence.EFCore;
using JualIn.App.Mobile.Core.Persistence.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.Messaging;
using JualIn.App.Mobile.Core.Infrastructure.Behaviors;



#if USE_OFFLINE_MODE
using JualIn.App.Mobile.Core.Infrastructure.Api.Mock;
#else
using Refit;
#endif // USE_OFFLINE_MODE

namespace JualIn.App.Mobile.Core
{
    public static class DependencyInjection
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddCore(IConfiguration configuration, Action<ApplicationOptions> configure)
            {
                return services
                    .Configure(configure)
                    .AddBackendApi(configuration)
                    .AddLogging(configuration)
                    .AddDatabase(configuration)
                    .AddServices()
                    .AddMediator(options =>
                    {
                        options.Assemblies = [typeof(DependencyInjection)];
                        options.PipelineBehaviors = [typeof(ValidationBehavior<,>)];
                    });
            }

            private IServiceCollection AddLogging(IConfiguration configuration)
            {
                services.AddLogging(builder
                    => builder.AddSerilog(new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .CreateLogger()));

                return services;
            }

            private IServiceCollection AddDatabase(IConfiguration configuration)
            {

                return services
                    .Configure<SqliteConfigurations>(configuration.GetSection(SqliteConfigurations.Section))
                    .AddEfCorePersistence()
                    .AddDbContext<AppDbContext>((provider, builder) =>
                    {
                        var config = provider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
                        var sqliteConfig = provider.GetRequiredService<IOptions<SqliteConfigurations>>().Value;

                        var databasePath = Path.Combine(config.DatabasePathPrefix, sqliteConfig.DatabaseName);
                        var connectionString = configuration.GetConnectionString(SqliteConfigurations.Section)?
                            .Replace(sqliteConfig.DataSourceKey, databasePath);

                        var conn = new SqliteConnection(connectionString);

                        conn.Open();

                        // apply encryption key
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = $"PRAGMA key = '{sqliteConfig.Password}';";
                            cmd.ExecuteNonQuery();
                        }

                        builder.UseSqlite(conn,
                            o =>
                            {
                                o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                                o.MigrationsAssembly(config.MigrationAssemblyName);
                            });
                    })

                .AddScoped<IOrderUnitOfWork, OrderUnitOfWork>()
                .AddScoped<ICategoryResolver<Inventory>, InventoryRepository>()
                .AddScoped<ISearchable<Inventory>, InventoryRepository>()
                .AddScopedRepository<IProductRepository, ProductRepository, Product>()
                .AddScopedRepository<IInventoryRepository, InventoryRepository, Inventory>()
                .AddScopedRepository<IStockMovementRepository, StockMovementRepository, StockMovement>()
                .AddScopedRepository<IOrderRepository, OrderRepository, Order>()
                .AddScopedRepository<IOrderTransactionRepository, OrderTransactionRepository, OrderTransaction>();
            }

            private IServiceCollection AddBackendApi(IConfiguration configuration)
            {
                services.Configure<ApiSettings>(configuration.GetSection(ApiSettings.Section));
#if USE_OFFLINE_MODE
                services.AddSingleton<IBackendApi, BackendApiMock>();
#else
                services.AddRefitClient<IBackendApi>(
                    service => GlobalRefitSettings.Default,
                    httpClientName: IBackendApi.ClientName)
                    .ConfigureHttpClient(
                        (provider, client) =>
                        {
                            var config = provider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
                            var settings = provider.GetRequiredService<IOptions<ApiSettings>>().Value;

#if DEBUG
                            var builder = new UriBuilder(settings.BackendUrl);

                            if (config.UseEmulator)
                            {
                                builder.Host = "10.0.2.2";
                                builder.Port = 5022;

                                client.BaseAddress = new Uri(builder.ToString());
                            }
                            else
#endif // DEBUG
                            {
                                client.BaseAddress = new Uri(settings.BackendUrl);
                            }

                            client.Timeout = TimeSpan.FromSeconds(60);
                        })
                    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                    {
#if DEBUG
                        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
#endif // DEBUG
                    });
#endif // USE_OFFLINE_MODE

                return services;
            }

            private IServiceCollection AddServices()
            {
                return services
                .AddSingleton<IAuthService, AuthService>()
                .AddSingleton<ITokenService, AuthService>()
                .AddSingleton<IUserService, AuthService>()
                // Catalogs
                .AddSingleton<IProductService, ProductService>()
                .AddScoped<ICategoryResolver<Product>, ProductRepository>()
                .AddScoped<ISearchable<Product>, ProductRepository>()
                // Inventories
                .AddTransient<StockMovementAggregate>()
                .AddTransient<StockMovementFactory>()
                .AddSingleton<IInventoryService, InventoryService>()
                // Payments
                .AddTransient<CashPayment>()
                .AddTransient<PaymentFactory>()
                .AddSingleton<IPaymentService, PaymentService>()
                // Sales
                .AddTransient<OrderTransactionFactory>()
                // Messaging
                .AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>()
                .AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
            }
        }
    }
}
