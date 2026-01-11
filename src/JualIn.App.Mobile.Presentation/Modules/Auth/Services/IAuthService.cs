namespace JualIn.App.Mobile.Presentation.Modules.Auth.Services
{
    public interface IAuthService : ITokenService, IUserService
    {
        Task SaveSignInDataAsync(string userIdentity, string accessToken, string? refreshToken, DateTime? expiration, CancellationToken cancellationToken = default);
    }
}
