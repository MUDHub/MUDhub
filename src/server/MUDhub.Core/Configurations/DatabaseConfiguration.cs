namespace MUDhub.Core.Configurations
{
    public class DatabaseConfiguration
    {
        public DatabaseProvider Provider { get; set; } = DatabaseProvider.Sqlite;
        public string ConnectionString { get; set; } = "Data Source=mudDatabase.db";
        public string DefaultMudAdminEmail { get; set; } = "admin@mud.de";
        public string DefaultMudAdminPassword { get; set; } = "admin";

    }
}