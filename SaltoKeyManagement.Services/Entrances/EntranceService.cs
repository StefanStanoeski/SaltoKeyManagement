using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SaltoKeyManagement.Data;
using SaltoKeyManagement.Models.Domain;
using SaltoKeyManagement.Models.Interfaces.Services.Entrances;
using SaltoKeyManagement.Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltoKeyManagement.Services.Entrances
{
    public class EntranceService : BaseService, IEntranceServiceAsync
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<IdentityUser> _userManager;

        public EntranceService(DataContext dataContext, UserManager<IdentityUser> userManager, TokenValidationParameters tokenValidationParameters)
            :base(tokenValidationParameters)
        {
            _dataContext = dataContext;
            _userManager = userManager;
        }

        public async Task<AuthorizationResult> OpenDoorForUser(string doorId, string token)
        {
            var door = await _dataContext.DoorsDbSet.SingleOrDefaultAsync(d => d.Id == Guid.Parse(doorId));

            if (door == null)
            {
                return new AuthorizationResult
                {
                    ErrorMessages = new[] { "No such door exists." }
                };
            }

            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
            {
                return new AuthorizationResult
                {
                    ErrorMessages = new[] { "Invalid token." }
                };
            }

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(c => c.Type == "id").Value);
            
            var entrance = new Entrance
            {
                UserId = user.Id,
                TimeOfEntry = DateTime.UtcNow,
                DoorId = Guid.Parse(doorId)
            };

            await _dataContext.EntrancesDbSet.AddAsync(entrance);
            var created = await _dataContext.SaveChangesAsync();

            var authResult = new AuthorizationResult
            {
                DoorName = door.Name,
                Token = token
            };

            return created > 0 ? authResult : new AuthorizationResult
            {
                ErrorMessages = new[] { "New entrance record failed to be entered." }
            };
        }
    }
}
