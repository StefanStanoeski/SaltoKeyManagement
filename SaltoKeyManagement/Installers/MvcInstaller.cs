using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SaltoKeyManagement.Models.Interfaces.Installers;
using SaltoKeyManagement.Models.Options;
using System.Collections.Generic;
using System.Text;
using Microsoft.OpenApi.Models;
using SaltoKeyManagement.Models.Contracts;

namespace SaltoKeyManagement.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(JwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameters;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(ClearanceClaims.Policy.GuestEntrance, builder => builder.RequireClaim(ClearanceClaims.User.Guest, "true"));
                options.AddPolicy(ClearanceClaims.Policy.EmployeeEntrance, builder => builder.RequireClaim(ClearanceClaims.User.Employee, "true"));
                options.AddPolicy(ClearanceClaims.Policy.MaintenanceEntrance, builder => builder.RequireClaim(ClearanceClaims.User.Maintenance, "true"));
                options.AddPolicy(ClearanceClaims.Policy.ServerStorageEntrance, builder => builder.RequireClaim(ClearanceClaims.User.ServerStorage, "true"));
                options.AddPolicy(ClearanceClaims.Policy.AdministrationEntrance, builder => builder.RequireClaim(ClearanceClaims.User.Administration, "true"));
                options.AddPolicy(ClearanceClaims.Policy.DocumentArchiveEntrance, builder => builder.RequireClaim(ClearanceClaims.User.DocumentArchive, "true"));
                options.AddPolicy(ClearanceClaims.Policy.ManagementEntrance, builder => builder.RequireClaim(ClearanceClaims.User.Management, "true"));
            });

            services.AddSwaggerGen(g =>
            {
                g.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Salto API", Version = "v1" });

                g.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                { 
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                g.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}
