namespace AuthDemo;

/// <summary>
/// AuthService contains the business logic we want to protect with QA-led automation.
/// The key teaching idea:
/// - QA "owns" policies (password rules and lockout threshold).
/// - Tests encode those policies.
/// - CI enforces those tests as quality gates.
/// </summary>
public sealed class AuthService
{
    private readonly IUserStore _store;

    /// <summary>
    /// QA-owned security policy:
    /// The account is locked after this many consecutive failed logins.
    ///
    /// In the demo, we intentionally change this value later to show CI failing.
    /// </summary>
    public const int LockoutThreshold = 6;

    public AuthService(IUserStore store)
    {
        _store = store;
    }

    /// <summary>
    /// QA-owned password policy.
    /// We keep it intentionally simple so students focus on CI + governance.
    /// Rules:
    /// - Must not be null/whitespace
    /// - Must be at least 8 characters
    /// - Must contain at least 1 digit
    /// </summary>
    public static bool IsPasswordValid(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        if (password.Length < 8)
            return false;

        if (!password.Any(char.IsDigit))
            return false;

        return true;
    }

    /// <summary>
    /// Registers a new user if the password policy passes.
    /// This supports unit-testable policy logic and integration-testable workflows.
    /// </summary>
    public bool Register(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            return false;

        if (!IsPasswordValid(password))
            return false;

        // Demo-only "hash" format.
        // Real systems use secure hashing and salts.
        var user = new UserAccount(username, $"HASH::{password}");

        _store.Upsert(user);
        return true;
    }

    /// <summary>
    /// Attempts login. Enforces:
    /// - Locked accounts cannot login
    /// - Failed logins increment failure count
    /// - After reaching threshold, account becomes locked
    /// - Successful login resets failures
    /// </summary>
    public bool Login(string username, string password)
    {
        var user = _store.Find(username);
        if (user is null)
            return false;

        // QA policy: locked means no login, even with correct password.
        if (user.IsLocked)
            return false;

        var isMatch = user.PasswordHash == $"HASH::{password}";

        if (!isMatch)
        {
            // QA policy enforcement: track failures and lock if needed.
            user.RegisterFailure(LockoutThreshold);
            _store.Upsert(user);
            return false;
        }

        // Successful login clears failures and unlocks.
        user.ResetFailures();
        _store.Upsert(user);

        return true;
    }
}
