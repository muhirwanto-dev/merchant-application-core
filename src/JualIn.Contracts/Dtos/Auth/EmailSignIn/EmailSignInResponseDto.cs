namespace JualIn.Contracts.Dtos.Auth.EmailSignIn
{
    public record EmailSignInResponseDto : AuthenticationResponseDto
    {
        public EmailSignInResponseDto(string AccessToken, string? RefreshToken, DateTime? Expiration, string UserIdentifier, bool IsEmailConfirmed)
            : base(AccessToken, RefreshToken, Expiration, UserIdentifier, IsEmailConfirmed)
        {
        }
    }
}
