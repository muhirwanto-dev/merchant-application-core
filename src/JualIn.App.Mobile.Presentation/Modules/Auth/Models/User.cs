namespace JualIn.App.Mobile.Presentation.Modules.Auth.Models
{
    public record User(
        string UserIdentity,
        string Username,
        string Email,
        string FirstName,
        string FullName)
    {
        public static readonly User Default = new(
            Guid.Empty.ToString(),
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty
            );
    }
}
