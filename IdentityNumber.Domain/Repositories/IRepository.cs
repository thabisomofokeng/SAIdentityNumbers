using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityNumber.Domain.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync();

        Task<T> AddAsync(T entity);
    }
}
