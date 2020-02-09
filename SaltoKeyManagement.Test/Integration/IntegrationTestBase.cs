using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SaltoKeyManagement.Data;
using SaltoKeyManagement.Models.Contracts;
using SaltoKeyManagement.Models.Contracts.Requests;
using SaltoKeyManagement.Models.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SaltoKeyManagement.Test.Integration
{
    public abstract class IntegrationTestBase
    {
        protected readonly HttpClient Client;

        public IntegrationTestBase()
        {
            //using in-memory Db instead of teardown and cleanup definitions
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder => 
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DataContext));
                        services.AddDbContext<DataContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });
                    });
                });
            Client = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync(string clearance)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync(clearance));
        }

        protected async Task<DoorResponse> CreateDoorAsync(CreateDoorRequest request)
        {
            var response = await Client.PostAsJsonAsync(ApiRoutes.Doors.Create, request);

            return await response.Content.ReadAsAsync<DoorResponse>();
        }

        private async Task<string> GetJwtAsync(string clearance)
        {
            var response = await Client.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest
            {
                Email = "user@integration.test",
                Clearance = clearance,
                Password = "Integration_01"
            });

            var registrationResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();

            return registrationResponse.Token;
        }
    }
}
