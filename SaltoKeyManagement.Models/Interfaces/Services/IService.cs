using System;
using System.Collections.Generic;
using System.Text;

namespace SaltoKeyManagement.Models.Interfaces.Services
{
    public interface IService<T>
    {
        ICollection<T> GetAll(int page, int pageSize);
        T GetById(Guid id);
        T Create(T item);
        T Update(T item);
        T Patch(T item);
        bool Delete(Guid id);
    }
}
