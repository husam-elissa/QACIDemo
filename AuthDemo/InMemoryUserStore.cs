using System.Collections.Concurrent;

namespace AuthDemo;

/// <summary>
/// Basic storage interface so our AuthService can be tested like a real workflow.
/// This supports our integration tests without requiring a database.
/// </summary>
public interface IUserStore
{
    UserAccount? Find(string username);
    void Upsert(UserAccount user);
}

/// <summary>
/// In-memory user store for demo purposes.
/// This simulates persistence and enables integration-style testing.
/// </summary>
public sealed class InMemoryUserStore : IUserStore
{
    // ConcurrentDictionary keeps it safe if tests ever run in parallel.
    private readonly ConcurrentDictionary<string, UserAccount> _users = new();

    public UserAccount? Find(string username)
    {
        return _users.TryGetValue(username, out var user) ? user : null;
    }

    public void Upsert(UserAccount user)
    {
        _users[user.Username] = user;
    }
}
