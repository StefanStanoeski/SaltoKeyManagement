using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SaltoKeyManagement.Data;
using SaltoKeyManagement.Models.Contracts.Requests;
using SaltoKeyManagement.Models.Contracts.Responses;
using SaltoKeyManagement.Models.Domain;
using SaltoKeyManagement.Models.Interfaces.Services.Entrances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltoKeyManagement.Services.Entrances
{
    public class EntranceService : IEntranceServiceAsync
    {
        private readonly DataContext _dataContext;

        public EntranceService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Entrance> OpenDoorForUser(string doorId, string userId)
        {
            var door = await _dataContext.DoorsDbSet.AsNoTracking().SingleOrDefaultAsync(d => d.Id == Guid.Parse(doorId));

            var entrance = new Entrance
            {
                UserId = userId,
                TimeOfEntry = DateTime.UtcNow,
                DoorId = door.Id
            };

            await _dataContext.EntrancesDbSet.AddAsync(entrance);
            var created = await _dataContext.SaveChangesAsync();

            return created > 0 ? entrance : null;
        }
    }
}
