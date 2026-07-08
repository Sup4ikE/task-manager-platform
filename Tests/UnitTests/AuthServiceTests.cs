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
        var userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
        var passwordHasher = new Mock<IPasswordHasher<User>>(MockBehavior.Strict);
        var jwtTokenGen = new Mock<IJwtTokenGenerator>(MockBehavior.Loose);
        var refreshTokenService = new Mock<IRefreshTokenService>(MockBehavior.Loose);

        const string username = "new_user";
        const string password = "Pass123!";

        userRepo
            .Setup(r => r.GetByUsernameAsync(username))
            .ReturnsAsync((User?)null);

        passwordHasher
            .Setup(h => h.HashPassword(It.IsAny<User>(), password))
            .Returns("hashed_password");

        userRepo
            .Setup(r => r.AddAsync(It.Is<User>(u =>
                u.Username == username &&
                u.Role == "User" &&
                u.PasswordHash == "hashed_password")))
            .ReturnsAsync((User u) => u);

        userRepo
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var sut = new AuthService(
            userRepo.Object,
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

        userRepo.Verify(r => r.GetByUsernameAsync(username), Times.Once);
        passwordHasher.Verify(h => h.HashPassword(It.IsAny<User>(), password), Times.Once);
        userRepo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        userRepo.Verify(r => r.SaveChangesAsync(), Times.Once);

        userRepo.VerifyNoOtherCalls();
        passwordHasher.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task UsernameAlreadyUsed()
    {
        // Arrange
        var userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
        var passwordHasher = new Mock<IPasswordHasher<User>>(MockBehavior.Strict);
        var jwtTokenGen = new Mock<IJwtTokenGenerator>(MockBehavior.Loose);
        var refreshTokenService = new Mock<IRefreshTokenService>(MockBehavior.Loose);

        const string username = "existing_user";
        const string password = "Pass123!";

        var existingUser = new User { Username = username };

        userRepo
            .Setup(r => r.GetByUsernameAsync(username))
            .ReturnsAsync(existingUser);

        var sut = new AuthService(
            userRepo.Object,
            passwordHasher.Object,
            jwtTokenGen.Object,
            refreshTokenService.Object);

        // Act
        var result = await sut.RegisterAsync(username, password);

        // Assert
        Assert.Null(result);

        userRepo.Verify(r => r.GetByUsernameAsync(username), Times.Once);
        userRepo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        userRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
        passwordHasher.Verify(h => h.HashPassword(It.IsAny<User>(), It.IsAny<string>()), Times.Never);

        userRepo.VerifyNoOtherCalls();
        passwordHasher.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task LoginSucceed()
    {
        // Arrange
        var userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
        var passwordHasher = new Mock<IPasswordHasher<User>>(MockBehavior.Strict);
        var jwtTokenGen = new Mock<IJwtTokenGenerator>(MockBehavior.Loose);
        var refreshTokenService = new Mock<IRefreshTokenService>(MockBehavior.Loose);

        const string username = "existing_user";
        const string password = "Pass123!";

        var user = new User { Id = 1, Username = username, PasswordHash = "hash", Role = "User" };

        userRepo
            .Setup(r => r.GetByUsernameAsync(username))
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
            userRepo.Object,
            passwordHasher.Object,
            jwtTokenGen.Object,
            refreshTokenService.Object);

        // Act
        var result = await sut.LoginAsync(username, password);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("jwt_token", result!.AccessToken);
        Assert.Equal("refresh_token", result.RefreshToken);

        userRepo.Verify(r => r.GetByUsernameAsync(username), Times.Once);
        passwordHasher.Verify(h => h.VerifyHashedPassword(user, user.PasswordHash, password), Times.Once);
        jwtTokenGen.Verify(g => g.CreateToken(user), Times.Once);
        refreshTokenService.Verify(r => r.GenerateAndSaveRefreshTokenAsync(user), Times.Once);

        userRepo.VerifyNoOtherCalls();
        passwordHasher.VerifyNoOtherCalls();
        jwtTokenGen.VerifyNoOtherCalls();
        refreshTokenService.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task LoginAsyncIncorrectPassword()
    {
        // Arrange
        var userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
        var passwordHasher = new Mock<IPasswordHasher<User>>(MockBehavior.Strict);
        var jwtTokenGen = new Mock<IJwtTokenGenerator>(MockBehavior.Loose);
        var refreshTokenService = new Mock<IRefreshTokenService>(MockBehavior.Loose);

        const string username = "existing_user";
        const string password = "Pass123!";

        var user = new User { Id = 1, Username = username, PasswordHash = "hash", Role = "User" };

        userRepo
            .Setup(r => r.GetByUsernameAsync(username))
            .ReturnsAsync(user);

        passwordHasher
            .Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, password))
            .Returns(PasswordVerificationResult.Failed);

        var sut = new AuthService(
            userRepo.Object,
            passwordHasher.Object,
            jwtTokenGen.Object,
            refreshTokenService.Object);

        // Act
        var result = await sut.LoginAsync(username, password);

        // Assert
        Assert.Null(result);

        userRepo.Verify(r => r.GetByUsernameAsync(username), Times.Once);
        passwordHasher.Verify(h => h.VerifyHashedPassword(user, user.PasswordHash, password), Times.Once);

        jwtTokenGen.Verify(g => g.CreateToken(It.IsAny<User>()), Times.Never);
        refreshTokenService.Verify(r => r.GenerateAndSaveRefreshTokenAsync(It.IsAny<User>()), Times.Never);

        userRepo.VerifyNoOtherCalls();
        passwordHasher.VerifyNoOtherCalls();
    }
}
