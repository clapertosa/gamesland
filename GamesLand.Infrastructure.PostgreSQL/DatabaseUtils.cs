namespace GamesLand.Infrastructure.PostgreSQL;

public class DatabaseUtils
{
    public static string GetDatabaseUrlFormatted(string url)
    {
        var urlCopy = url.Replace("postgres://", "");
        var usernameAndPassword = urlCopy[..urlCopy.IndexOf('@')].Split(':');
        var hostAndPort = urlCopy[(urlCopy.IndexOf('@') + 1)..urlCopy.IndexOf('/')].Split(':');
        var databaseName = urlCopy[(urlCopy.LastIndexOf('/') + 1)..urlCopy.Length];

        return
            $"Username = {usernameAndPassword[0]};" +
            $"Password = {usernameAndPassword[1]};" +
            $"Server = {hostAndPort[0]};" +
            $"Port = {hostAndPort[1]};" +
            $"Database = {databaseName};" +
            "SSL Mode = Prefer;" +
            "Trust Server Certificate = true";
    }
}