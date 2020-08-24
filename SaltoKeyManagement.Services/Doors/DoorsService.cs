using Microsoft.EntityFrameworkCore;
using SaltoKeyManagement.Data;
using SaltoKeyManagement.Models.Domain;
using SaltoKeyManagement.Models.Interfaces.Services.Doors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltoKeyManagement.Services.Doors
{
    public class DoorsService : IDoorsServiceAsync
    {
        private readonly DataContext _dataContext;

        public DoorsService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Door> CreateAsync(Door item)
        {
            var door = await GetByIdAsync(item.Id);

            if (door != null)
            {
                return null;
            }

            await _dataContext.DoorsDbSet.AddAsync(item);
            var created = await _dataContext.SaveChangesAsync();

            return created > 0 ? item : null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var door = await GetByIdAsync(id);

            if (door == null)
            {
                return false;
            }

            _dataContext.DoorsDbSet.Remove(door);
            var deleted = await _dataContext.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<ICollection<Door>> GetAllAsync(int page, int pageSize)
        {
            return await _dataContext.DoorsDbSet
                .AsQueryable()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Door> GetByIdAsync(Guid id)
        {
            return await _dataContext.DoorsDbSet
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Door> PatchAsync(Door item)
        {
            return await UpdateAsync(item);
        }

        public async Task<Door> UpdateAsync(Door item)
        {
            var door = await GetByIdAsync(item.Id);

            if (door == null)
            {
                return null;
            }

            _dataContext.DoorsDbSet.Update(item);
            var updated = await _dataContext.SaveChangesAsync();

            return updated > 0 ? item : null;
        }
    }
}
