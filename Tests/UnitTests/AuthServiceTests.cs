using Microsoft.AspNetCore.Identity;
using Moq;
using TaskManager_API.Core.Application.Interfaces;
using TaskManager_API.Core.Application.Services;
using TaskManager_API.Core.Domain;

namespace Tests.Unit;

public class AuthServiceTests
{
    [Fact]
    public async Task RegisterAsync_WhenUsernameIsNew_CreatesUserAndSaves()
    {
        // Arrange
        var unitofwork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var passwordHasher = new Mock<IPasswordHasher<User>>(MockBehavior.Strict);
        var jwtTokenGen = new Mock<IJwtTokenGenerator>(MockBehavior.Loose);
        var refreshTokenService = new Mock<IRefreshTokenService>(MockBehavior.Loose);

        const string username = "new_user";
        const string password = "Pass123!";

        unitofwork
            .Setup(r => r.Users.GetByUsernameAsync(username))
            .ReturnsAsync((User?)null);

        passwordHasher
            .Setup(h => h.HashPassword(It.IsAny<User>(), password))
            .Returns("hashed_password");

        unitofwork
            .Setup(r => r.Users.AddAsync(It.Is<User>(u =>
                u.Username == username &&
                u.Role == "User" &&
                u.PasswordHash == "hashed_password")))
            .ReturnsAsync((User u) => u);

        unitofwork
            .Setup(r => r.SaveChangesAsync())
            .ReturnsAsync(1);

        var sut = new AuthService(
            unitofwork.Object,
            passwordHasher.Object,
            jwtTokenGen.Object,
            refreshTokenService.Object);

        // Act
        var createdUser = await sut.RegisterAsync(username, password);

        // Assert
        Assert.NotNull(createdUser);
        Assert.Equal(username, createdUser!.Username);
        Assert.Equal("User", createdUser.Role);
        Assert.False(string.IsNullOrWhiteSpace(createdUser.PasswordHash));

        unitofwork.Verify(r => r.Users.GetByUsernameAsync(username), Times.Once);
        passwordHasher.Verify(h => h.HashPassword(It.IsAny<User>(), password), Times.Once);
        unitofwork.Verify(r => r.Users.AddAsync(It.IsAny<User>()), Times.Once);
        unitofwork.Verify(r => r.SaveChangesAsync(), Times.Once);

        unitofwork.VerifyNoOtherCalls();
        passwordHasher.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task UsernameAlreadyUsed()
    {
        // Arrange
        var unitofwork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var passwordHasher = new Mock<IPasswordHasher<User>>(MockBehavior.Strict);
        var jwtTokenGen = new Mock<IJwtTokenGenerator>(MockBehavior.Loose);
        var refreshTokenService = new Mock<IRefreshTokenService>(MockBehavior.Loose);

        const string username = "existing_user";
        const string password = "Pass123!";

        var existingUser = new User { Username = username };

        unitofwork
            .Setup(r => r.Users.GetByUsernameAsync(username))
            .ReturnsAsync(existingUser);

        var sut = new AuthService(
            unitofwork.Object,
            passwordHasher.Object,
            jwtTokenGen.Object,
            refreshTokenService.Object);

        // Act
        var result = await sut.RegisterAsync(username, password);

        // Assert
        Assert.Null(result);

        unitofwork.Verify(r => r.Users.GetByUsernameAsync(username), Times.Once);
        unitofwork.Verify(r => r.Users.AddAsync(It.IsAny<User>()), Times.Never);
        unitofwork.Verify(r => r.SaveChangesAsync(), Times.Never);
        passwordHasher.Verify(h => h.HashPassword(It.IsAny<User>(), It.IsAny<string>()), Times.Never);

        unitofwork.VerifyNoOtherCalls();
        passwordHasher.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task LoginSucceed()
    {
        // Arrange
        var unitofwork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var passwordHasher = new Mock<IPasswordHasher<User>>(MockBehavior.Strict);
        var jwtTokenGen = new Mock<IJwtTokenGenerator>(MockBehavior.Loose);
        var refreshTokenService = new Mock<IRefreshTokenService>(MockBehavior.Loose);

        const string username = "existing_user";
        const string password = "Pass123!";

        var user = new User { Id = 1, Username = username, PasswordHash = "hash", Role = "User" };

        unitofwork
            .Setup(r => r.Users.GetByUsernameAsync(username))
            .ReturnsAsync(user);

        passwordHasher
            .Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, password))
            .Returns(PasswordVerificationResult.Success);

        jwtTokenGen
            .Setup(g => g.CreateToken(user))
            .Returns("jwt_token");

        refreshTokenService
            .Setup(r => r.GenerateAndSaveRefreshTokenAsync(user))
            .ReturnsAsync("refresh_token");

        var sut = new AuthService(
            unitofwork.Object,
            passwordHasher.Object,
            jwtTokenGen.Object,
            refreshTokenService.Object);

        // Act
        var result = await sut.LoginAsync(username, password);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("jwt_token", result!.AccessToken);
        Assert.Equal("refresh_token", result.RefreshToken);

        unitofwork.Verify(r => r.Users.GetByUsernameAsync(username), Times.Once);
        passwordHasher.Verify(h => h.VerifyHashedPassword(user, user.PasswordHash, password), Times.Once);
        jwtTokenGen.Verify(g => g.CreateToken(user), Times.Once);
        refreshTokenService.Verify(r => r.GenerateAndSaveRefreshTokenAsync(user), Times.Once);

        unitofwork.VerifyNoOtherCalls();
        passwordHasher.VerifyNoOtherCalls();
        jwtTokenGen.VerifyNoOtherCalls();
        refreshTokenService.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task LoginAsyncIncorrectPassword()
    {
        // Arrange
        var unitofwork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        var passwordHasher = new Mock<IPasswordHasher<User>>(MockBehavior.Strict);
        var jwtTokenGen = new Mock<IJwtTokenGenerator>(MockBehavior.Loose);
        var refreshTokenService = new Mock<IRefreshTokenService>(MockBehavior.Loose);

        const string username = "existing_user";
        const string password = "Pass123!";

        var user = new User { Id = 1, Username = username, PasswordHash = "hash", Role = "User" };

        unitofwork
            .Setup(r => r.Users.GetByUsernameAsync(username))
            .ReturnsAsync(user);

        passwordHasher
            .Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, password))
            .Returns(PasswordVerificationResult.Failed);

        var sut = new AuthService(
            unitofwork.Object,
            passwordHasher.Object,
            jwtTokenGen.Object,
            refreshTokenService.Object);

        // Act
        var result = await sut.LoginAsync(username, password);

        // Assert
        Assert.Null(result);

        unitofwork.Verify(r => r.Users.GetByUsernameAsync(username), Times.Once);
        passwordHasher.Verify(h => h.VerifyHashedPassword(user, user.PasswordHash, password), Times.Once);

        jwtTokenGen.Verify(g => g.CreateToken(It.IsAny<User>()), Times.Never);
        refreshTokenService.Verify(r => r.GenerateAndSaveRefreshTokenAsync(It.IsAny<User>()), Times.Never);

        unitofwork.VerifyNoOtherCalls();
        passwordHasher.VerifyNoOtherCalls();
    }
}
