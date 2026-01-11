using JualIn.App.Mobile.Data.Configurations;
using JualIn.App.Mobile.Data.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SingleScope.Persistence.EFCore;

namespace JualIn.App.Mobile.Data
{
    public static class DependencyInjection
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddAppData(IConfiguration configuration, string? databasePathPrefix = null)
            {
                return services
                    .AddDatabase(configuration, databasePathPrefix);
            }

            private IServiceCollection AddDatabase(IConfiguration configuration, string? databasePathPrefix = null)
            {
                return services
                    .Configure<SqliteConfigurations>(configuration.GetSection(SqliteConfigurations.Section))
                    .AddEfCorePersistence()
                    .AddDbContext<AppDbContext>((provider, builder) =>
                    {
                        var config = provider.GetRequiredService<IOptions<SqliteConfigurations>>().Value;
                        var databasePath = databasePathPrefix == null
                            ? config.DatabaseName
                            : Path.Combine(databasePathPrefix, config.DatabaseName);
                        var connectionString = configuration.GetConnectionString(SqliteConfigurations.Section)?
                            .Replace(config.DataSourceKey, databasePath);

                        var conn = new SqliteConnection(connectionString);

                        conn.Open();

                        // apply encryption key
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = $"PRAGMA key = '{config.Password}';";
                            cmd.ExecuteNonQuery();
                        }

                        builder.UseSqlite(conn,
                            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
                    });
            }
        }
    }
}
