using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SaltoKeyManagement.Models.Interfaces.Services
{
    public interface IServiceAsync<T>
    {
        Task<ICollection<T>> GetAllAsync(int page, int pageSize);
        Task<T> GetByIdAsync(Guid id);
        Task<T> CreateAsync(T item);
        Task<T> UpdateAsync(T item);
        Task<T> PatchAsync(T item);
        Task<bool> DeleteAsync(Guid id);
    }
}
