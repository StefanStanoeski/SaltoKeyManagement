using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaltoKeyManagement.Models.Contracts;
using SaltoKeyManagement.Models.Contracts.Requests;
using SaltoKeyManagement.Models.Contracts.Responses;
using SaltoKeyManagement.Models.Domain;
using SaltoKeyManagement.Models.Interfaces.Services.Doors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaltoKeyManagement.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DoorsController : Controller
    {
        private IDoorsServiceAsync _doorsService;

        public DoorsController(IDoorsServiceAsync doorsService)
        {
            _doorsService = doorsService;
        }

        [HttpGet(ApiRoutes.Doors.GetAll)]
        public async Task<IActionResult> GetAll([FromRoute] int pageNumber = 1, [FromRoute] int pageSize = 10)
        {
            return Ok(await _doorsService.GetAllAsync(pageNumber, pageSize));
        }

        [HttpGet(ApiRoutes.Doors.Get)]
        public async Task<IActionResult> GetById([FromRoute] string doorId)
        {
            if (doorId == null)
            {
                return BadRequest();
            }

            var user = await _doorsService.GetByIdAsync(Guid.Parse(doorId));

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpDelete(ApiRoutes.Doors.Delete)]
        public async Task<IActionResult> Delete([FromRoute] string doorId)
        {
            if (doorId == null)
            {
                return BadRequest();
            }

            var isDeleteSuccessful = await _doorsService.DeleteAsync(Guid.Parse(doorId));

            if (!isDeleteSuccessful)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost(ApiRoutes.Doors.Create)]
        public async Task<IActionResult> Create([FromBody] CreateDoorRequest doorToCreate)
        {
            var doorToAdd = new Door
            {
                Name = doorToCreate.Name
            };

            var createdDoor = await _doorsService.CreateAsync(doorToAdd);

            if (createdDoor == null)
            {
                return BadRequest();
            }

            var doorResponseObject = new DoorResponse
            {
                Id = createdDoor.Id.ToString(),
                Name = createdDoor.Name
            };

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";

            var locationUri = $"{baseUrl}/{ApiRoutes.Doors.Get.Replace("{doorId}", doorResponseObject.Id)}";

            return Created(locationUri, doorResponseObject);
        }

        [HttpPut(ApiRoutes.Doors.Update)]
        public async Task<IActionResult> Update([FromRoute] string doorId, [FromBody] UpdateDoorRequest door)
        {
            var doorToUpdate = new Door
            {
                Id = Guid.Parse(doorId),
                Name = door.Name
            };

            var updatedDoor = await _doorsService.UpdateAsync(doorToUpdate);

            if (updatedDoor == null)
            {
                return NotFound();
            }

            var doorResponseObject = new DoorResponse
            {
                Id = updatedDoor.Id.ToString(),
                Name = updatedDoor.Name
            };

            return Ok(doorResponseObject);
        }
    }
}
