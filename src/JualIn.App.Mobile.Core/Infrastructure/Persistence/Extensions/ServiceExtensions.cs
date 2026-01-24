using JualIn.App.Mobile.Core.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JualIn.App.Mobile.Core.Infrastructure.Persistence.Extensions
{
    public static class ServiceExtensions
    {
        extension(IServiceProvider provider)
        {
            public void MigrateDatabase()
            {
                using var scope = provider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                dbContext.Database.Migrate();
            }
        }
    }
}
