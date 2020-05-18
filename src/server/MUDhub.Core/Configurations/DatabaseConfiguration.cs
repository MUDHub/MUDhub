namespace MUDhub.Core.Configurations
{
    public class DatabaseConfiguration
    {
        public DatabaseProvider Provider { get; set; } = DatabaseProvider.Sqlite;
        public string ConnectionString { get; set; } = string.Empty;
        public string DefaultMudAdminEmail { get; set; } = string.Empty;
        public string DefaultMudAdminPassword { get; set; } = string.Empty;
        public string DefaultMudMasterEmail { get; set; } = string.Empty;
        public string DefaultMudMasterPassword { get; set; } = string.Empty;
        public bool CreateDefaultAdminUser { get; set; } = false;
        public bool CreateDefaultMasterUser { get; set; } = false;
        public bool CreateDefaultDhbwMudData { get; set; } = false;
        public bool DeleteDatabase { get; set; } = false;
    }
}