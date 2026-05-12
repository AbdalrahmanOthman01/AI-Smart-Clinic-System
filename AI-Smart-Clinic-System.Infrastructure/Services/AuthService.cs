using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AI_Smart_Clinic_System.Application.Dtos;
using AI_Smart_Clinic_System.Application.Interfaces;
using AI_Smart_Clinic_System.Application.Options;
using AI_Smart_Clinic_System.Application.User.CreateUser;
using AI_Smart_Clinic_System.Domain.Entities;

namespace AI_Smart_Clinic_System.Infrastructure.Services;

internal sealed class AuthService(
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService,
    IOptions<JWTOptions> options) : IAuthService
{
    public async Task<Result<AuthResult>> RegisterAsync(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        var existingUser = await userManager.Users
            .SingleOrDefaultAsync(u => u.PhoneNumber == command.PhoneNumber, cancellationToken);

        if (existingUser is not null)
        {
            return Error.InvalidCredentials("Auth.PhoneAlreadyRegistered", "This phone number is already registered.");
        }

        var user = new ApplicationUser
        {
            UserName = command.PhoneNumber,
            FirstName = command.FirstName,
            LastName = command.LastName,
            PhoneNumber = command.PhoneNumber,
        };

        var identityResult = await userManager.CreateAsync(user, command.Password);

        if (!identityResult.Succeeded)
        {
            var errors = identityResult.Errors
                .Select(e => Error.Failure(e.Code, e.Description))
                .ToList();

            return errors;
        }

        var claims = await userManager.GetClaimsAsync(user);

        var token = tokenService.CreateJwtToken(user, claims);
        var refreshToken = tokenService.GenerateRefreshToken();
        user.RefreshTokens.Add(refreshToken);
        await userManager.UpdateAsync(user);

        var authResult = new AuthResult
        {
            PhoneNumber = user.PhoneNumber,
            Token = token,
            IsAuthenticated = true,
            ExpiresOn = DateTime.UtcNow.AddHours(options.Value.Duration),
            Username = user.UserName,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpiresOn = refreshToken.ExpiresOn
        };

        return authResult;
    }

    public async Task<Result<AuthResult>> GetTokenAsync(string phoneNumber, string password, CancellationToken cancellationToken = default)
    {
        var user = await userManager.Users
            .SingleOrDefaultAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);

        if (user is null)
        {
            return Error.InvalidCredentials("Auth.InvalidCredentials", "Phone number or password is incorrect.");
        }

        var passwordValid = await userManager.CheckPasswordAsync(user, password);

        if (!passwordValid)
        {
            return Error.InvalidCredentials("Auth.InvalidCredentials", "Phone number or password is incorrect.");
        }

        var authResult = new AuthResult();

        var userClaims = await userManager.GetClaimsAsync(user);
        var token = tokenService.CreateJwtToken(user, userClaims);

        if (user.RefreshTokens.Any(rf => rf.IsActive))
        {
            var activeRefreshToken = user.RefreshTokens.First(rf => rf.IsActive);
            authResult.RefreshToken = activeRefreshToken.Token;
            authResult.RefreshTokenExpiresOn = activeRefreshToken.ExpiresOn;
        }
        else
        {
            var newRefreshToken = tokenService.GenerateRefreshToken();
            authResult.RefreshToken = newRefreshToken.Token;
            authResult.RefreshTokenExpiresOn = newRefreshToken.ExpiresOn;
            user.RefreshTokens.Add(newRefreshToken);
            await userManager.UpdateAsync(user);
        }

        authResult.Username = user.UserName;
        authResult.PhoneNumber = user.PhoneNumber;
        authResult.Token = token;
        authResult.IsAuthenticated = true;
        authResult.ExpiresOn = DateTime.UtcNow.AddHours(options.Value.Duration);

        return authResult;
    }

    public async Task<Result<AuthResult>> GetRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var authModel = new AuthResult();

        var user = await userManager.Users
            .SingleOrDefaultAsync(u => u.RefreshTokens.Any(rf => rf.Token == refreshToken), cancellationToken);

        if (user is null)
        {
            return Error.InvalidCredentials("Auth.InvalidRefreshToken", "Invalid refresh token.");
        }

        var refreshTokenFromDb = user.RefreshTokens.First(rf => rf.Token == refreshToken);

        if (!refreshTokenFromDb.IsActive)
        {
            return Error.InvalidCredentials("Auth.InactiveRefreshToken", "Inactive refresh token.");
        }

        refreshTokenFromDb.RevokedOn = DateTime.UtcNow;

        var newRefreshToken = tokenService.GenerateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);
        await userManager.UpdateAsync(user);

        var claims = await userManager.GetClaimsAsync(user);
        var jwtToken = tokenService.CreateJwtToken(user, claims);

        authModel.IsAuthenticated = true;
        authModel.ExpiresOn = DateTime.UtcNow.AddHours(options.Value.Duration);
        authModel.Token = jwtToken;
        authModel.Username = user.UserName;
        authModel.RefreshToken = newRefreshToken.Token;
        authModel.RefreshTokenExpiresOn = newRefreshToken.ExpiresOn;

        return authModel;
    }

    public async Task<Result> RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var user = await userManager.Users
            .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken), cancellationToken);

        if (user is null)
        {
            return Error.InvalidCredentials("Auth.InvalidRefreshToken", "Invalid refresh token.");
        }

        var token = user.RefreshTokens.Single(rf => rf.Token == refreshToken);

        if (!token.IsActive)
        {
            return Error.InvalidCredentials("Auth.InactiveRefreshToken", "Refresh token is already inactive.");
        }

        token.RevokedOn = DateTime.UtcNow;
        await userManager.UpdateAsync(user);

        return Result.Ok();
    }
}

