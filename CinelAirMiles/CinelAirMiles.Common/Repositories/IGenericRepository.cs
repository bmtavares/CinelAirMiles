namespace CinelAirMiles.Common.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IGenericRepository<T> where T : class
    {

        IQueryable<T> GetAll();


        Task<T> GetByIdAsync(int id);


        Task CreateAsync(T entity);


        Task UpdateAsync(T entity);


        Task DeleteAsync(T entity);


        Task<bool> ExistsAsync(int id);
    }
}
