using SaltoKeyManagement.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SaltoKeyManagement.Models.Interfaces.Services.Identity
{
    public interface IIdentityServiceAsync
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password, string clearance);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    }
}
