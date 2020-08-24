using SaltoKeyManagement.Models.Contracts.Requests;
using SaltoKeyManagement.Models.Contracts.Responses;
using SaltoKeyManagement.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SaltoKeyManagement.Models.Interfaces.Services.Entrances
{
    public interface IEntranceServiceAsync
    {
        Task<Entrance> OpenDoorForUser(string doorId, string UserId);
    }
}
