using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaltoKeyManagement.Models.Contracts;
using SaltoKeyManagement.Models.Contracts.Responses;
using SaltoKeyManagement.Models.Domain;
using SaltoKeyManagement.Models.Interfaces.Services.Entrances;
using SaltoKeyManagement.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> OpenGuestDoorForUser([FromRoute] string doorId)
        {
            return await openDoorForUser(doorId);
        }

        [HttpGet(ApiRoutes.Entrances.OpenEmployeeDoorForUser)]
        [Authorize(Policy = ClearanceClaims.Policy.EmployeeEntrance)]
        public async Task<IActionResult> OpenEmployeeDoorForUser([FromRoute] string doorId)
        {
            return await openDoorForUser(doorId);
        }

        [HttpGet(ApiRoutes.Entrances.OpenMaintenanceDoorForUser)]
        [Authorize(Policy = ClearanceClaims.Policy.MaintenanceEntrance)]
        public async Task<IActionResult> OpenMaintenanceDoorForUser([FromRoute] string doorId)
        {
            return await openDoorForUser(doorId);
        }

        [HttpGet(ApiRoutes.Entrances.OpenServerStorageDoorForUser)]
        [Authorize(Policy = ClearanceClaims.Policy.ServerStorageEntrance)]
        public async Task<IActionResult> OpenServerStorageDoorForUser([FromRoute] string doorId)
        {
            return await openDoorForUser(doorId);
        }

        [HttpGet(ApiRoutes.Entrances.OpenAdministrationDoorForUser)]
        [Authorize(Policy = ClearanceClaims.Policy.AdministrationEntrance)]
        public async Task<IActionResult> OpenAdministrationDoorForUser([FromRoute] string doorId)
        {
            return await openDoorForUser(doorId);
        }

        [HttpGet(ApiRoutes.Entrances.OpenDocumentArchiveDoorForUser)]
        [Authorize(Policy = ClearanceClaims.Policy.DocumentArchiveEntrance)]
        public async Task<IActionResult> OpenDocumentArchiveDoorForUser([FromRoute] string doorId)
        {
            return await openDoorForUser(doorId);
        }

        [HttpGet(ApiRoutes.Entrances.OpenManagementDoorForUser)]
        [Authorize(Policy = ClearanceClaims.Policy.ManagementEntrance)]
        public async Task<IActionResult> OpenManagementDoorForUser([FromRoute] string doorId)
        {
            return await openDoorForUser(doorId);
        }

        private async Task<IActionResult> openDoorForUser(string doorId)
        {
            Entrance entrance = null;

            try
            {
                entrance = await _entranceService.OpenDoorForUser(doorId, HttpContext.GetUserId());
            }
            catch(Exception ex) when (ex is ArgumentNullException || ex is FormatException)
            {
                return BadRequest(new EntranceResponse { StatusMessage = "Invalid door identifier." });
            }
            catch (Exception ex) when (ex is NullReferenceException)
            {
                return BadRequest(new EntranceResponse { StatusMessage = "Error while creating entrance record." });
            }

            if (entrance == null)
            {
                return BadRequest(new EntranceResponse { StatusMessage = "No entrance record created." });
            }

            return Ok(new EntranceResponse { StatusMessage = "Authorization successful." });
        }
    }
}
