using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SaltoKeyManagement.Data;
using SaltoKeyManagement.Models.Contracts;
using SaltoKeyManagement.Models.Domain;
using SaltoKeyManagement.Models.Interfaces.Services.Identity;
using SaltoKeyManagement.Models.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SaltoKeyManagement.Services.Identity
{
    public class IdentityService : IIdentityServiceAsync
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly DataContext _dataContext;
        private TokenValidationParameters _tokenValidationParameters;

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters, DataContext dataContext)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _dataContext = dataContext;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] { "Wrong username or password." }
                };
            }

            var isPasswordValidForUser = await _userManager.CheckPasswordAsync(user, password);

            if (!isPasswordValidForUser)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] { "Wrong username or password." }
                };
            }

            return await GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string password, string clearance)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] { "User with this email address has already been registered." }
                };
            }

            var newUserId = Guid.NewGuid();

            var newUser = new IdentityUser
            {
                Id = newUserId.ToString(),
                Email = email,
                UserName = email
            };

            var createdUser = await _userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = createdUser.Errors.Select(e => e.Description)
                };
            }

            await _userManager.AddClaimAsync(newUser, new Claim(clearance, true.ToString().ToLower()));

            return await GenerateAuthenticationResultForUserAsync(newUser);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] { "Invalid token." }
                };
            }

            var expiryDateUnix = 
                long.Parse(validatedToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix)
                .AddHours(1);

            if (expiryDateTimeUtc > DateTime.Now)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] { "Too early to refresh token." }
                };
            }

            var jti = validatedToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _dataContext.RefreshTokensDbSet
                .SingleOrDefaultAsync(r => r.Token == refreshToken);

            if (storedRefreshToken == null)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] { "Refresh token does not exist." }
                };
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] { "Refresh token has expired." }
                };
            }

            if (storedRefreshToken.IsInvalidated)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] { "Refresh token has been invalidated." }
                };
            }

            if (storedRefreshToken.IsUsed)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] { "Refresh token has already been used." }
                };
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult
                {
                    ErrorMessages = new[] { "Refresh token does not match this JWT." }
                };
            }

            storedRefreshToken.IsUsed = true;

            _dataContext.RefreshTokensDbSet.Update(storedRefreshToken);

            await _dataContext.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(c => c.Type == "id").Value);

            return await GenerateAuthenticationResultForUserAsync(user);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id),
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(_jwtSettings.RefreshTokenIssuingLifetimeInMonths)
            };

            await _dataContext.RefreshTokensDbSet.AddAsync(refreshToken);
            await _dataContext.SaveChangesAsync();

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!isJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool isJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
