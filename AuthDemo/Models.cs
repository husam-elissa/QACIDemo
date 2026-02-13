namespace AuthDemo;

/// <summary>
/// Represents a user account in this demo.
/// This is intentionally simplified for teaching QA-driven automation and CI gates.
/// In production, you would not store passwords like this, and you would likely use an identity provider.
/// </summary>
public sealed class UserAccount
{
    public string Username { get; }

    // Demo-only password "hash".
    // This is NOT secure and is used only to keep the demo focused on QA automation strategy.
    public string PasswordHash { get; }

    // Tracks consecutive failed login attempts.
    public int FailedAttempts { get; private set; }

    // Indicates whether the account is locked due to failed attempts.
    public bool IsLocked { get; private set; }

    public UserAccount(string username, string passwordHash)
    {
        Username = username;
        PasswordHash = passwordHash;
    }

    /// <summary>
    /// Records a failed login attempt.
    /// QA policy: once FailedAttempts reaches the lockout threshold, the user is locked.
    /// </summary>
    public void RegisterFailure(int lockoutThreshold)
    {
        FailedAttempts++;

        // Lock the account when failures reach or exceed the threshold.
        if (FailedAttempts >= lockoutThreshold)
        {
            IsLocked = true;
        }
    }

    /// <summary>
    /// Resets failures after a successful login.
    /// QA policy: successful authentication clears failed attempts and unlocks the account.
    /// </summary>
    public void ResetFailures()
    {
        FailedAttempts = 0;
        IsLocked = false;
    }
}
