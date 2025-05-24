using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotoGen.Domain.Interfaces
{
    public interface IModelRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByDestinationAsync(string name);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
