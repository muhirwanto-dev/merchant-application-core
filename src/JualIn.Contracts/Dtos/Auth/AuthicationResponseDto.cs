namespace JualIn.Contracts.Dtos.Auth
{
    public record AuthenticationResponseDto(
        string AccessToken,
        string? RefreshToken,
        DateTime? Expiration,
        string UserIdentifier,
        bool IsEmailConfirmed
        );
}
