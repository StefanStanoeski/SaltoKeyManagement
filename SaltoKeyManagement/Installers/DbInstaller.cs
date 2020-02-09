using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaltoKeyManagement.Data;
using SaltoKeyManagement.Models.Interfaces.Installers;
using SaltoKeyManagement.Models.Interfaces.Services.Doors;
using SaltoKeyManagement.Models.Interfaces.Services.Entrances;
using SaltoKeyManagement.Models.Interfaces.Services.Identity;
using SaltoKeyManagement.Services.Doors;
using SaltoKeyManagement.Services.Entrances;
using SaltoKeyManagement.Services.Identity;

namespace SaltoKeyManagement.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddScoped<IIdentityServiceAsync, IdentityService>();
            services.AddScoped<IDoorsServiceAsync, DoorsService>();
            services.AddScoped<IEntranceServiceAsync, EntranceService>();
        }
    }
}
