using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaltoKeyManagement.Models.Contracts;
using SaltoKeyManagement.Models.Contracts.Responses;
using SaltoKeyManagement.Models.Domain;
using SaltoKeyManagement.Models.Interfaces.Services.Entrances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationResult = SaltoKeyManagement.Models.Domain.AuthorizationResult;

namespace SaltoKeyManagement.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EntrancesController : Controller
    {
        private readonly IEntranceServiceAsync _entranceService;

        public EntrancesController(IEntranceServiceAsync entranceService)
        {
            _entranceService = entranceService;
        }

        [HttpGet(ApiRoutes.Entrances.OpenGuestDoorForUser)]
        [Authorize(Policy = ClearanceClaims.Policy.GuestEntrance)]
        public async Task<IActionResult> OpenGuestDoorForUser([FromRoute] string doorId, [FromRoute] string token)
        {
            var authResponse = await _entranceService.OpenDoorForUser(doorId, token);

            return getActionBasedOnAuthorisationResult(authResponse);
        }

        [HttpGet(ApiRoutes.Entrances.OpenEmployeeDoorForUser)]
        [Authorize(Policy = ClearanceClaims.Policy.EmployeeEntrance)]
        public async Task<IActionResult> OpenEmployeeDoorForUser([FromRoute] string doorId, [FromRoute] string token)
        {
            var authResponse = await _entranceService.OpenDoorForUser(doorId, token);

            return getActionBasedOnAuthorisationResult(authResponse);
        }

        [HttpGet(ApiRoutes.Entrances.OpenMaintenanceDoorForUser)]
        [Authorize(Policy = ClearanceClaims.Policy.MaintenanceEntrance)]
        public async Task<IActionResult> OpenMaintenanceDoorForUser([FromRoute] string doorId, [FromRoute] string token)
        {
            var authResponse = await _entranceService.OpenDoorForUser(doorId, token);

            return getActionBasedOnAuthorisationResult(authResponse);
        }

        [HttpGet(ApiRoutes.Entrances.OpenServerStorageDoorForUser)]
        [Authorize(Policy = ClearanceClaims.Policy.ServerStorageEntrance)]
        public async Task<IActionResult> OpenServerStorageDoorForUser([FromRoute] string doorId, [FromRoute] string token)
        {
            var authResponse = await _entranceService.OpenDoorForUser(doorId, token);

            return getActionBasedOnAuthorisationResult(authResponse);
        }

        [HttpGet(ApiRoutes.Entrances.OpenAdministrationDoorForUser)]
        [Authorize(Policy = ClearanceClaims.Policy.AdministrationEntrance)]
        public async Task<IActionResult> OpenAdministrationDoorForUser([FromRoute] string doorId, [FromRoute] string token)
        {
            var authResponse = await _entranceService.OpenDoorForUser(doorId, token);

            return getActionBasedOnAuthorisationResult(authResponse);
        }

        [HttpGet(ApiRoutes.Entrances.OpenDocumentArchiveDoorForUser)]
        [Authorize(Policy = ClearanceClaims.Policy.DocumentArchiveEntrance)]
        public async Task<IActionResult> OpenDocumentArchiveDoorForUser([FromRoute] string doorId, [FromRoute] string token)
        {
            var authResponse = await _entranceService.OpenDoorForUser(doorId, token);

            return getActionBasedOnAuthorisationResult(authResponse);
        }

        [HttpGet(ApiRoutes.Entrances.OpenManagementDoorForUser)]
        [Authorize(Policy = ClearanceClaims.Policy.ManagementEntrance)]
        public async Task<IActionResult> OpenManagementDoorForUser([FromRoute] string doorId, [FromRoute] string token)
        {
            var authResponse = await _entranceService.OpenDoorForUser(doorId, token);

            return getActionBasedOnAuthorisationResult(authResponse);
        }

        private IActionResult getActionBasedOnAuthorisationResult(AuthorizationResult authResult)
        {
            if (!authResult.Success)
            {
                return BadRequest(new EntranceResponse
                {
                    StatusMessage = "You are not authorised to use this door."
                });
            }

            return Ok(new EntranceResponse
            {
                StatusMessage = "Authorisation successful."
            });
        }
    }
}
