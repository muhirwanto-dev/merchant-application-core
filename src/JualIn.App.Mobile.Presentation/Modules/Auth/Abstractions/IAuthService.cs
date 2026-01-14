namespace JualIn.App.Mobile.Presentation.Modules.Auth.Abstractions
{
    public interface IAuthService : ITokenService, IUserService
    {
        Task SaveSignInDataAsync(string userIdentity, string accessToken, string? refreshToken, DateTime? expiration, CancellationToken cancellationToken = default);
    }
}
