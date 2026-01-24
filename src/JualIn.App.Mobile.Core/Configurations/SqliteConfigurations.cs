namespace JualIn.App.Mobile.Core.Configurations
{
    public sealed class SqliteConfigurations
    {
        public const string Section = "SQLite";

        public string DatabaseName { get; set; } = string.Empty;

        public string DataSourceKey { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
