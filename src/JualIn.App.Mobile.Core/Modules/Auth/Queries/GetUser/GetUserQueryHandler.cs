using JualIn.App.Mobile.Core.Modules.Auth.Abstractions;
using JualIn.App.Mobile.Core.Modules.Auth.Models;
using Mediator;

namespace JualIn.App.Mobile.Core.Modules.Auth.Queries.GetUser
{
    public class GetUserQueryHandler(
        IUserService _userService) : IQueryHandler<GetUserQuery, User?>
    {
        public async ValueTask<User?> Handle(GetUserQuery query, CancellationToken cancellationToken) => await Task.FromResult(_userService.HasUserData ? _userService.User : null);
    }
}
