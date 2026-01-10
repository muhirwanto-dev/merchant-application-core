namespace JualIn.Infrastructures.Configurations
{
    public record SqliteConfigurations(
        string DatabaseName,
        string DataSourceKey)
    {
        public const string Section = "SQLite";
    }
}
