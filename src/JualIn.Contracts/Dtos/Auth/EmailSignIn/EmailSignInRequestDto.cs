namespace JualIn.Contracts.Dtos.Auth.EmailSignIn
{
    public record EmailSignInRequestDto(
        string Email,
        string Password,
        bool RememberMe
        );
}
