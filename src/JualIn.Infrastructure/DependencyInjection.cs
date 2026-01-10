using JualIn.Application.Persistence.Repositories;
using JualIn.Domain.Inventories.Entities;
using JualIn.Infrastructure.Mailing;
using JualIn.Infrastructure.Persistence.Contexts;
using JualIn.Infrastructure.Persistence.Repositories;
using JualIn.Infrastructures.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using SingleScope.Persistence;
using SingleScope.Persistence.EFCore;

namespace JualIn.Infrastructure
{
    public static class DependencyInjection
    {
        private static bool _useSqlite = false;
        private static bool _usePostgreSql = false;
        private static string? _databasePathPrefix = null;

        extension(IServiceCollection services)
        {
            public IServiceCollection AddInfrastructure(IConfiguration configuration)
            {
                return services
                    .AddLogging(configuration)
                    .AddPersistence(configuration)
                    .AddMailing(configuration);
            }

            public IServiceCollection UseSqlite(string? databasePathPrefix = null)
            {
                _useSqlite = true;
                _databasePathPrefix = databasePathPrefix;

                return services;
            }

            public IServiceCollection UsePostgreSql(string? databasePathPrefix = null)
            {
                _usePostgreSql = true;
                _databasePathPrefix = databasePathPrefix;

                return services;
            }

            private IServiceCollection AddPersistence(IConfiguration configuration)
            {
                services
                    .AddEfCorePersistence()
                    .AddDbContext<AppDbContext>((provider, builder) =>
                    {
                        if (_usePostgreSql)
                        {
                            // todo
                        }

                        if (_useSqlite)
                        {
                            var options = provider.GetRequiredService<IOptions<SqliteConfigurations>>().Value;
                            var connectionString = configuration.GetConnectionString("SQLite") ?? string.Empty;
                            var databasePath = _databasePathPrefix == null
                                ? options.DatabaseName
                                : Path.Combine(_databasePathPrefix, options.DatabaseName);

                            connectionString = connectionString.Replace(options.DataSourceKey, databasePath);

                            builder.UseSqlite(connectionString,
                                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
                        }
                    });

                services.AddScopedRepository<ISearchableRepository<Inventory>, InventoryRepository, Inventory>();

                return services;
            }

            private IServiceCollection AddLogging(IConfiguration configuration)
            {
                services.AddLogging(builder => builder.AddSerilog(new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger()));

                return services;
            }

            private IServiceCollection AddMailing(IConfiguration configuration)
            {
                services.Configure<EmailConfiguration>(configuration.GetSection(EmailConfiguration.Section));

                return services;
            }
        }
    }
}
