using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;
using Wolverine.FluentValidation;

namespace JualIn.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWolverine(opt =>
            {
                opt.UseFluentValidation();
                opt.Policies.UseDurableOutboxOnAllSendingEndpoints();
            });

            return services;
        }
    }
}
