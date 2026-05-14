using CareerLens.Domain.Entities;
using FluentAssertions;

namespace CareerLens.Tests.Domain;

public class UserEntityTests
{
    [Fact]
    public void Create_GecerliVeriler_KullaniciOlusturur()
    {
        var user = User.Create("Test@Example.COM", "hash");

        user.Email.Should().Be("test@example.com");
        user.PasswordHash.Should().Be("hash");
        user.Plan.Should().Be(UserPlan.Free);
        user.RefreshToken.Should().BeNull();
    }

    [Fact]
    public void SetRefreshToken_Token_AtamaYapar()
    {
        var user = User.Create("test@example.com", "hash");
        var expiry = DateTime.UtcNow.AddDays(30);

        user.SetRefreshToken("token123", expiry);

        user.RefreshToken.Should().Be("token123");
        user.RefreshTokenExpiresAt.Should().Be(expiry);
    }

    [Fact]
    public void RevokeRefreshToken_Token_TemizleniR()
    {
        var user = User.Create("test@example.com", "hash");
        user.SetRefreshToken("token123", DateTime.UtcNow.AddDays(30));

        user.RevokeRefreshToken();

        user.RefreshToken.Should().BeNull();
        user.RefreshTokenExpiresAt.Should().BeNull();
    }

    [Fact]
    public void UpgradePlan_Pro_PlanGuncellenir()
    {
        var user = User.Create("test@example.com", "hash");
        user.UpgradePlan(UserPlan.Pro);
        user.Plan.Should().Be(UserPlan.Pro);
    }
}
