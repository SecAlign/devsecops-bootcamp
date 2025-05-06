// // Created On: 2025.05.06
// // Create by: althunibat

namespace Bootcamp.WebApi.Config;

public static class Extensions {
    public static string GetDbConnection(this IConfiguration config) {
        return
            $"Host={config.GetDbHost()};Port={config.GetDbPort()};Database={config.GetDbName()};Username={config.GetDbUser()};Password={config.GetDbPassword()};";
    }

    private static string GetDbHost(this IConfiguration config) {
        return config[ConfigurationKeys.DbHost] ?? "localhost";
    }

    private static string GetDbPort(this IConfiguration config) {
        return config[ConfigurationKeys.DbPort] ?? "5432";
    }

    private static string GetDbUser(this IConfiguration config) {
        return config[ConfigurationKeys.DbUser] ?? "postgres";
    }

    private static string GetDbPassword(this IConfiguration config) {
        return config[ConfigurationKeys.DbPassword] ?? "postgres";
    }

    private static string GetDbName(this IConfiguration config) {
        return config[ConfigurationKeys.DbName] ?? "api_db";
    }

    public static string GetDbSchema(this IConfiguration config) {
        return config[ConfigurationKeys.DbSchema] ?? "api";
    }
    private static class ConfigurationKeys {
        internal const string DbSchema = "DB_SCHEMA";
        internal const string DbHost = "DB_HOST";
        internal const string DbPort = "DB_PORT";
        internal const string DbUser = "DB_USER";
        internal const string DbPassword = "DB_PWD";
        internal const string DbName = "DB_NAME";
    }
}