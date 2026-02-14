using FluentAssertions;
using Xunit;

namespace AuthDemo.Tests.Policy;

/// <summary>
/// PolicyEnforcementTests exist to enforce non-negotiable QA policies.
/// 
/// These tests do NOT validate workflow behavior.
/// They validate approved policy values that must not change without formal approval.
/// 
/// In this demo, the approved security policy states:
/// "Accounts must lock after exactly five consecutive failed login attempts."
/// 
/// If a developer modifies the LockoutThreshold constant in AuthService,
/// this test will fail and block the change through Continuous Integration.
/// 
/// This represents QA governance enforcement.
/// </summary>
public sealed class PolicyEnforcementTests
{
    /// <summary>
    /// This test ensures that the LockoutThreshold constant
    /// matches the officially approved QA security requirement.
    /// 
    /// Why this test exists:
    /// - Developers may attempt to change the threshold (e.g., from 5 to 6).
    /// - Business stakeholders may request a usability adjustment.
    /// - A misunderstanding of requirements may introduce unintended change.
    /// 
    /// Regardless of the reason, the approved policy must not change silently.
    /// 
    /// If the constant value differs from 5, this test fails,
    /// and the CI pipeline blocks the change.
    /// </summary>
    [Fact]
    public void LockoutThreshold_should_match_approved_policy_value()
    {
        // Approved QA policy value.
        const int approvedPolicyValue = 5;

        // Assert that the implementation matches the approved policy.
        AuthDemo.AuthService.LockoutThreshold
            .Should()
            .Be(
                 approvedPolicyValue,
                 "the approved security policy requires lockout after exactly five failed login attempts"
          );

    }
}
