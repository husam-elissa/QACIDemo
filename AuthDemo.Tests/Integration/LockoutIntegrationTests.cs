using FluentAssertions;
using Xunit;

namespace AuthDemo.Tests.Integration;

/// <summary>
/// Integration tests validate end-to-end behavior across multiple components.
/// Here we validate:
/// - registration
/// - repeated failed logins
/// - lockout after the QA policy threshold
/// - reset behavior on successful login
/// </summary>
public sealed class LockoutIntegrationTests
{
    [Fact]
    public void User_should_lock_after_five_failed_logins()
    {
        var store = new AuthDemo.InMemoryUserStore();
        var auth = new AuthDemo.AuthService(store);

        // Register a known user.
        auth.Register("sam", "Password1").Should().BeTrue();

        // QA policy requirement: lock after exactly 5 failed attempts.
        for (int i = 0; i < 5; i++)
        {
            auth.Login("sam", "WrongPass1").Should().BeFalse();
        }

        // Once locked, even correct password should fail.
        auth.Login("sam", "Password1")
        .Should()
        .BeFalse("the account should be locked after five failed login attempts");
    }

    [Fact]
    public void Successful_login_should_reset_failed_attempts()
    {
        var store = new AuthDemo.InMemoryUserStore();
        var auth = new AuthDemo.AuthService(store);

        auth.Register("sam", "Password1").Should().BeTrue();

        // One failure happens.
        auth.Login("sam", "WrongPass1").Should().BeFalse();

        // Successful login resets failures and unlocks.
        auth.Login("sam", "Password1").Should().BeTrue();

        // Now do threshold-4 failures; user should NOT be locked yet.
        for (int i = 0; i < AuthDemo.AuthService.LockoutThreshold - 4; i++)
        {
            auth.Login("sam", "WrongPass1").Should().BeFalse();
        }

        // Correct password should still succeed, proving reset worked earlier.
        auth.Login("sam", "Password1").Should().BeTrue();
    }
}
