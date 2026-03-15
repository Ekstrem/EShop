namespace EShop.IntegrationTests;

using Xunit;
using Hive.SeedWorks.TacticalPatterns;
using Hive.SeedWorks.Result;

using Customer.Domain.Abstraction;
using Customer.Domain.Implementation;
using Session.Domain.Abstraction;
using Session.Domain.Implementation;

/// <summary>
/// Integration tests verifying the customer onboarding journey across
/// Customer and Session bounded contexts.
/// Journey: Register customer -> Verify email -> Create session -> Refresh session
/// </summary>
public class CustomerOnboardingJourneyTests
{
    [Fact]
    public void FullOnboarding_RegisterCustomer_VerifyEmail_CreateSession_RefreshToken()
    {
        // Step 1: Register a new customer
        var registerResult = CustomerAggregate.RegisterCustomer(
            email: "jane.doe@example.com",
            firstName: "Jane",
            lastName: "Doe",
            passwordHash: "hashed_password_abc123");

        Assert.True(registerResult.IsSuccess);
        Assert.NotNull(registerResult.Model);
        Assert.Equal("jane.doe@example.com", registerResult.Model!.Root.Email);
        Assert.Equal("Jane", registerResult.Model.Root.FirstName);
        Assert.Equal("Doe", registerResult.Model.Root.LastName);
        Assert.Equal("Unverified", registerResult.Model.Root.Status);

        // Step 2: Verify email (transitions from Unverified to Active)
        var verifyResult = CustomerAggregate.VerifyEmail(registerResult.Model);

        Assert.True(verifyResult.IsSuccess);
        Assert.NotNull(verifyResult.Model);
        Assert.Equal("Active", verifyResult.Model!.Root.Status);
        Assert.Equal("jane.doe@example.com", verifyResult.Model.Root.Email);
        Assert.Equal("Jane", verifyResult.Model.Root.FirstName);

        // Step 3: Create a login session for the verified customer
        // The customer ID comes from the concrete model
        var customerId = (verifyResult.Model as CustomerAnemicModel)!.Id;
        var sessionResult = SessionAggregate.CreateSession(
            customerId: customerId,
            token: "jwt-token-initial-xyz",
            duration: TimeSpan.FromHours(2),
            deviceInfo: "Chrome 120 / Windows 11");

        Assert.True(sessionResult.IsSuccess);
        Assert.NotNull(sessionResult.Model);
        Assert.Equal("Active", sessionResult.Model!.Root.Status);
        Assert.Equal(customerId, sessionResult.Model.Root.CustomerId);
        Assert.Equal("jwt-token-initial-xyz", sessionResult.Model.Root.Token);
        Assert.Equal("Chrome 120 / Windows 11", sessionResult.Model.Root.DeviceInfo);
        Assert.True(sessionResult.Model.Root.ExpiresAt > DateTime.UtcNow);

        // Step 4: Refresh the session token (extends expiration)
        var refreshResult = SessionAggregate.RefreshSession(
            current: sessionResult.Model,
            newToken: "jwt-token-refreshed-abc",
            duration: TimeSpan.FromHours(2));

        Assert.True(refreshResult.IsSuccess);
        Assert.NotNull(refreshResult.Model);
        Assert.Equal("Active", refreshResult.Model!.Root.Status);
        Assert.Equal("jwt-token-refreshed-abc", refreshResult.Model.Root.Token);
        Assert.Equal(customerId, refreshResult.Model.Root.CustomerId);
        // Refreshed session should expire later than original
        Assert.True(refreshResult.Model.Root.ExpiresAt >= sessionResult.Model.Root.ExpiresAt);
    }

    [Fact]
    public void Onboarding_RegisterThenDeactivate_AnonymizesPII()
    {
        // Register and verify
        var registerResult = CustomerAggregate.RegisterCustomer(
            email: "to-delete@example.com",
            firstName: "Alice",
            lastName: "Smith",
            passwordHash: "hashed_pwd");

        Assert.True(registerResult.IsSuccess);

        var verifyResult = CustomerAggregate.VerifyEmail(registerResult.Model!);
        Assert.True(verifyResult.IsSuccess);
        Assert.Equal("Active", verifyResult.Model!.Root.Status);

        // Deactivate the account
        var deactivateResult = CustomerAggregate.DeactivateAccount(verifyResult.Model);

        Assert.True(deactivateResult.IsSuccess);
        Assert.Equal("Deactivated", deactivateResult.Model!.Root.Status);
        Assert.Equal("Anonymized", deactivateResult.Model.Root.FirstName);
        Assert.Equal("User", deactivateResult.Model.Root.LastName);
        Assert.Contains("@anonymized.local", deactivateResult.Model.Root.Email);
        Assert.Equal(string.Empty, deactivateResult.Model.Root.PasswordHash);
    }

    [Fact]
    public void Onboarding_CreateMultipleSessions_ThenRevokeAll()
    {
        // Register and verify customer
        var registerResult = CustomerAggregate.RegisterCustomer(
            email: "multi-session@example.com",
            firstName: "Bob",
            lastName: "Jones",
            passwordHash: "pwd_hash");
        var verifyResult = CustomerAggregate.VerifyEmail(registerResult.Model!);
        var customerId = (verifyResult.Model as CustomerAnemicModel)!.Id;

        // Create session from desktop
        var session1 = SessionAggregate.CreateSession(
            customerId, "token-desktop-1", TimeSpan.FromHours(1), "Chrome / Desktop");
        Assert.True(session1.IsSuccess);

        // Create session from mobile
        var session2 = SessionAggregate.CreateSession(
            customerId, "token-mobile-1", TimeSpan.FromHours(1), "Safari / iPhone");
        Assert.True(session2.IsSuccess);

        // Revoke all sessions
        var activeSessions = new List<ISessionAnemicModel>
        {
            session1.Model!,
            session2.Model!
        };
        var revokeResults = SessionAggregate.RevokeAllSessions(activeSessions);

        Assert.Equal(2, revokeResults.Count);
        Assert.All(revokeResults, r =>
        {
            Assert.True(r.IsSuccess);
            Assert.Equal("Revoked", r.Model!.Root.Status);
        });
    }

    [Fact]
    public void Onboarding_ChangePasswordAfterVerification()
    {
        // Register, verify, then change password
        var registerResult = CustomerAggregate.RegisterCustomer(
            email: "changepwd@example.com",
            firstName: "Charlie",
            lastName: "Brown",
            passwordHash: "old_password_hash");

        var verifyResult = CustomerAggregate.VerifyEmail(registerResult.Model!);
        Assert.Equal("Active", verifyResult.Model!.Root.Status);

        var changeResult = CustomerAggregate.ChangePassword(
            verifyResult.Model,
            newPasswordHash: "new_secure_password_hash");

        Assert.True(changeResult.IsSuccess);
        Assert.Equal("new_secure_password_hash", changeResult.Model!.Root.PasswordHash);
        Assert.Equal("Active", changeResult.Model.Root.Status);
        Assert.Equal("changepwd@example.com", changeResult.Model.Root.Email);
    }

    [Fact]
    public void Onboarding_UpdateConsentAfterVerification()
    {
        var registerResult = CustomerAggregate.RegisterCustomer(
            email: "consent@example.com",
            firstName: "Dana",
            lastName: "White",
            passwordHash: "pwd");

        var verifyResult = CustomerAggregate.VerifyEmail(registerResult.Model!);

        // Grant marketing consent
        var consentResult = CustomerAggregate.UpdateConsent(
            verifyResult.Model!,
            consentType: "Marketing",
            isGranted: true);

        Assert.True(consentResult.IsSuccess);
        Assert.Single(consentResult.Model!.Consents);
        Assert.Equal("Marketing", consentResult.Model.Consents[0].ConsentType);
        Assert.True(consentResult.Model.Consents[0].IsGranted);

        // Revoke marketing consent
        var revokeConsent = CustomerAggregate.UpdateConsent(
            consentResult.Model,
            consentType: "Marketing",
            isGranted: false);

        Assert.True(revokeConsent.IsSuccess);
        Assert.Single(revokeConsent.Model!.Consents);
        Assert.False(revokeConsent.Model.Consents[0].IsGranted);
    }
}
