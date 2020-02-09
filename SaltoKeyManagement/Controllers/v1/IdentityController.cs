using Microsoft.AspNetCore.Mvc;
using SaltoKeyManagement.Models.Contracts;
using SaltoKeyManagement.Models.Contracts.Requests;
using SaltoKeyManagement.Models.Contracts.Responses;
using SaltoKeyManagement.Models.Domain;
using SaltoKeyManagement.Models.Interfaces.Services.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaltoKeyManagement.Controllers.v1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityServiceAsync _identityService;

        public IdentityController(IIdentityServiceAsync identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest regRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            var authResponse = await _identityService.RegisterAsync(regRequest.Email, regRequest.Password, regRequest.Clearance);

            return getActionBasedOnAthenticationResult(authResponse);
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest loginRequest)
        {
            var authResponse = await _identityService.LoginAsync(loginRequest.Email, loginRequest.Password);

            return getActionBasedOnAthenticationResult(authResponse);
        }

        [HttpPost(ApiRoutes.Identity.Refresh)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshRequest)
        {
            var authResponse = await _identityService
                .RefreshTokenAsync(refreshRequest.Token, refreshRequest.RefreshToken);

            return getActionBasedOnAthenticationResult(authResponse);
        }

        private IActionResult getActionBasedOnAthenticationResult(AuthenticationResult authResult)
        {
            if (!authResult.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResult.ErrorMessages
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResult.Token,
                RefreshToken = authResult.RefreshToken
            });
        }
    }
}
