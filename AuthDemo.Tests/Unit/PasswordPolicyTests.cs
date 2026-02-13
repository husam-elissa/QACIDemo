using FluentAssertions;
using Xunit;

namespace AuthDemo.Tests.Unit;

/// <summary>
/// Unit tests validate password policy rules in isolation.
/// These tests are fast and should run early in the CI pipeline.
/// </summary>
public sealed class PasswordPolicyTests
{
    [Theory]
    [InlineData("short1")]              // too short
    [InlineData("longbutnodigits")]     // no digit
    [InlineData("")]                    // empty
    [InlineData("        ")]            // whitespace
    public void Invalid_passwords_should_fail(string password)
    {
        AuthDemo.AuthService.IsPasswordValid(password).Should().BeFalse();
    }

    [Theory]
    [InlineData("Password1")]
    [InlineData("abcDEF12")]
    public void Valid_passwords_should_pass(string password)
    {
        AuthDemo.AuthService.IsPasswordValid(password).Should().BeTrue();
    }
}
