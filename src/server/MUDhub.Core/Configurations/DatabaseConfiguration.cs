namespace MUDhub.Core.Configurations
{
    public class DatabaseConfiguration
    {
        public DatabaseProvider Provider { get; set; } = DatabaseProvider.Sqlite;
        public string ConnectionString { get; set; } = string.Empty;
        public string DefaultMudAdminEmail { get; set; } = string.Empty;
        public string DefaultMudAdminPassword { get; set; } = string.Empty;
        public bool CreateDefaultUser { get; set; } = false;
        public bool CreateDefaultMudData { get; set; } = false;
        public bool DeleteDatabase { get; set; } = false;
    }
}