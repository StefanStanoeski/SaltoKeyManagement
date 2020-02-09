using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SaltoKeyManagement.Models.Contracts;
using SaltoKeyManagement.Models.Contracts.Requests;
using SaltoKeyManagement.Models.Contracts.Responses;
using SaltoKeyManagement.Models.Domain;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SaltoKeyManagement.Test.Integration
{
    public class DoorControllerTests : IntegrationTestBase
    {
        [Fact]
        public async Task CreateDoor_DoorDoesNotExist_ReturnCreatedDoor()
        {
            //Arrange
            await AuthenticateAsync(ClearanceClaims.User.Guest);
            var doorName = "A_Door";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Doors.Create, new CreateDoorRequest { Name = doorName });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var door = await response.Content.ReadAsAsync<DoorResponse>();

            door.Name.Should().Be(doorName);
        }

        [Fact]
        public async Task UpdateDoor_DoorDoesNotExist_ReturnNotFound()
        {
            //Arrange
            await AuthenticateAsync(ClearanceClaims.User.Guest);
            var doorName = "A_Door";

            //Act
            var response = await Client
                .PutAsJsonAsync(ApiRoutes.Doors.Update.Replace("{doorId}", Guid.NewGuid().ToString()), new UpdateDoorRequest { Name = doorName });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateDoor_DoorDoesExist_ReturnUpdatedDoor()
        {
            //Arrange
            await AuthenticateAsync(ClearanceClaims.User.Guest);
            var doorName = "A_Door";

            //Act
            var createResponse = await Client.PostAsJsonAsync(ApiRoutes.Doors.Create, new CreateDoorRequest { Name = doorName });

            var door = await createResponse.Content.ReadAsAsync<DoorResponse>();

            doorName = "Updated_Door_Name";

            var response = await Client
                .PutAsJsonAsync(ApiRoutes.Doors.Update.Replace("{doorId}", door.Id), new UpdateDoorRequest { Name = doorName });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedDoor = await response.Content.ReadAsAsync<DoorResponse>();

            updatedDoor.Id.Should().Be(door.Id);
            updatedDoor.Name.Should().Be(doorName);
        }

        [Fact]
        public async Task GetById_WithoutAnyDoors_ReturnNotFound()
        {
            //Arrange
            await AuthenticateAsync(ClearanceClaims.User.Guest);

            //Act
            var response = await Client.GetAsync(ApiRoutes.Doors.Get.Replace("{doorId}", Guid.NewGuid().ToString()));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_DoorExists_ReturnDoor()
        {
            //Arrange
            await AuthenticateAsync(ClearanceClaims.User.Guest);

            var door = await CreateDoorAsync(new CreateDoorRequest 
            {
                Name = "TestDoor"
            });

            //Act
            var response = await Client.GetAsync(ApiRoutes.Doors.Get.Replace("{doorId}", door.Id));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var returnedDoor = await response.Content.ReadAsAsync<Door>();

            returnedDoor.Id.Should().Be(door.Id);
            returnedDoor.Name.Should().Be(door.Name);
        }

        [Fact]
        public async Task GetAll_WithoutAnyDoors_ReturnEmptyResponse()
        {
            //Arrange
            await AuthenticateAsync(ClearanceClaims.User.Guest);

            //Act
            var response = await Client.GetAsync(ApiRoutes.Doors.GetAll);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<List<Door>>()).Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_WithDoors_ReturnListOfDoors()
        {
            //Arrange
            await AuthenticateAsync(ClearanceClaims.User.Guest);

            var door = await CreateDoorAsync(new CreateDoorRequest
            {
                Name = "TestDoor"
            });

            //Act
            var response = await Client.GetAsync(ApiRoutes.Doors.GetAll);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var returnedDoors = await response.Content.ReadAsAsync<List<Door>>();

            foreach (var returnedDoor in returnedDoors)
            {
                returnedDoor.Id.Should().Be(door.Id);
                returnedDoor.Name.Should().Be(door.Name);
            }
        }

        [Fact]
        public async Task Delete_WithoutAnyDoors_ReturnNotFound()
        {
            //Arrange
            await AuthenticateAsync(ClearanceClaims.User.Guest);

            //Act
            var response = await Client.DeleteAsync(ApiRoutes.Doors.Delete.Replace("{doorId}", Guid.NewGuid().ToString()));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_WithDoors_ReturnNoContent()
        {
            //Arrange
            await AuthenticateAsync(ClearanceClaims.User.Guest);

            var door = await CreateDoorAsync(new CreateDoorRequest
            {
                Name = "TestDoor"
            });

            //Act
            var response = await Client.DeleteAsync(ApiRoutes.Doors.Delete.Replace("{doorId}", door.Id));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
