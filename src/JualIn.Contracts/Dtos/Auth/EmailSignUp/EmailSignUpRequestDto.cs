using JualIn.Contracts.Enums;

namespace JualIn.Contracts.Dtos.Auth.EmailSignUp
{
    public record EmailSignUpRequestDto(
        string Email,
        string Password,
        string FirstName,
        string? LastName,
        string? BusinessName,
        BusinessCategory? BusinessCategory
        );
}
