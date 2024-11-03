using System.Collections.Concurrent;

public class UsersManager
{
    private static readonly ConcurrentDictionary<string, string> _users = new();

    public static void AgregarUsuario(string connectionId)
    {
        _users[connectionId] = Guid.NewGuid().ToString();
    }

    public static void RemoverUsuario(string connectionId)
    {
        _users.TryRemove(connectionId, out _);
    }

    public static List<string> ObtenerUsuarios()
    {
        return _users.Keys.ToList();
    }
}
