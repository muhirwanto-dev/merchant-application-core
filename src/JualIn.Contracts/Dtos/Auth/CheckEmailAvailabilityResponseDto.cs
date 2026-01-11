namespace JualIn.Contracts.Dtos.Auth
{
    public record CheckEmailAvailabilityResponseDto(
        bool IsAvailable,
        string Message
        );
}
