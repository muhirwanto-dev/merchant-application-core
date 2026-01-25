using JualIn.App.Mobile.Core.Modules.Auth.Models;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Auth.Queries.GetUser
{
    public record GetUserQuery : IQuery<User?>;
}
