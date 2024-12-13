using System;
using Xunit;
using SmaHauJenHoaVij.Core.Domain.Users;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithValidData()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var name = "John Doe";
        var email = "john.doe@example.com";
        var passwordHash = "hashedpassword";

        // Act
        var user = new TestUser(userId, name, email, passwordHash);

        // Assert
        Assert.Equal(userId, user.Id);
        Assert.Equal(name, user.Name);
        Assert.Equal(email, user.Email);
        Assert.Equal(passwordHash, user.PasswordHash);
        Assert.Null(user.PasswordResetToken);
        Assert.Null(user.PasswordResetTokenExpiry);
    }

    [Fact]
    public void SetPasswordResetToken_ShouldUpdateTokenAndExpiry()
    {
        // Arrange
        var user = new TestUser(Guid.NewGuid(), "John Doe", "john.doe@example.com", "hashedpassword");
        var token = "reset-token";
        var expiry = DateTime.UtcNow.AddHours(1);

        // Act
        user.SetPasswordResetToken(token, expiry);

        // Assert
        Assert.Equal(token, user.PasswordResetToken);
        Assert.Equal(expiry, user.PasswordResetTokenExpiry);
    }

    [Fact]
    public void ClearPasswordResetToken_ShouldClearTokenAndExpiry()
    {
        // Arrange
        var user = new TestUser(Guid.NewGuid(), "John Doe", "john.doe@example.com", "hashedpassword");
        user.SetPasswordResetToken("reset-token", DateTime.UtcNow.AddHours(1));

        // Act
        user.ClearPasswordResetToken();

        // Assert
        Assert.Null(user.PasswordResetToken);
        Assert.Null(user.PasswordResetTokenExpiry);
    }

    private class TestUser : User
    {
        public TestUser(Guid id, string name, string email, string passwordHash)
        {
            Id = id;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
        }

        public void SetPasswordResetToken(string token, DateTime expiry)
        {
            PasswordResetToken = token;
            PasswordResetTokenExpiry = expiry;
        }

        public void ClearPasswordResetToken()
        {
            PasswordResetToken = null;
            PasswordResetTokenExpiry = null;
        }
    }
}