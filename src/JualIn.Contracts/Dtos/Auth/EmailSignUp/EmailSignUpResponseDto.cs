namespace JualIn.Contracts.Dtos.Auth.EmailSignUp
{
    public record EmailSignUpResponseDto : AuthenticationResponseDto
    {
        public EmailSignUpResponseDto(string AccessToken, string? RefreshToken, DateTime? Expiration, string UserIdentifier, bool IsEmailConfirmed)
            : base(AccessToken, RefreshToken, Expiration, UserIdentifier, IsEmailConfirmed)
        {
        }
    }
}
