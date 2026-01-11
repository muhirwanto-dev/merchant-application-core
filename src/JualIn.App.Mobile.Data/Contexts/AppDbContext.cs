using Mediator;
using Microsoft.EntityFrameworkCore;

namespace JualIn.App.Mobile.Data.Contexts
{
    public sealed class AppDbContext(
        DbContextOptions _options,
        IMediator _mediator
        ) : MigratableAppDbContext(_options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new DomainEventsDbInterceptor(_mediator));
        }
    }
}
