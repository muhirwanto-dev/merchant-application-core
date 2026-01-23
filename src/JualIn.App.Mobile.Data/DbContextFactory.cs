using JualIn.App.Mobile.Data.Configurations;
using JualIn.App.Mobile.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JualIn.App.Mobile.Data
{
    public class DbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            var options = new SqliteConfigurations
            {
                DatabaseName = "belibu.db",
                DataSourceKey = "{datasource}"
            };

            optionsBuilder.UseSqlite($"Data Source={options.DatabaseName};Password=belibu.app",
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
