namespace JualIn.Infrastructure.Mailing
{
    public record EmailConfiguration(
        string FromEmail,
        string FromName,
        string SubjectTemplate)
    {
        public const string Section = "EmailConfiguration";
    }
}
