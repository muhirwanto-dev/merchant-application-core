using JualIn.App.Mobile.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JualIn.App.Mobile.Data.Extensions
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
