using JualIn.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JualIn.Infrastructure.Persistence.Extensions
{
    public static class DatabaseExtensions
    {
        extension(IServiceProvider provider)
        {
            public void MigrateDatabase()
            {
                using var scope = provider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                context.Database.Migrate();
            }
        }
    }
}
