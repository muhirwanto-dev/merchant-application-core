namespace JualIn.Contracts.Dtos.Account
{
    public record GetUserInformationResponseDto(
        string Username,
        string Email,
        string FirstName,
        string FullName
        );
}
