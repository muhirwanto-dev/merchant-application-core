using JualIn.App.Mobile.Data.Configurations;
using JualIn.App.Mobile.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JualIn.App.Mobile.Data
{
    public class DbContextFactory : IDesignTimeDbContextFactory<MigratableAppDbContext>
    {
        public MigratableAppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MigratableAppDbContext>();
            var options = new SqliteConfigurations
            {
                DatabaseName = "belibu.db",
                DataSourceKey = "{datasource}"
            };

            optionsBuilder.UseSqlite($"Data Source={options.DatabaseName};Password=belibu.app",
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));

            return new MigratableAppDbContext(optionsBuilder.Options);
        }
    }
}
