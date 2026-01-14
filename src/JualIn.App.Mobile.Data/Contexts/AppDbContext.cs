using Microsoft.EntityFrameworkCore;

namespace JualIn.App.Mobile.Data.Contexts
{
    public sealed class AppDbContext(DbContextOptions _options) : MigratableAppDbContext(_options);
}
