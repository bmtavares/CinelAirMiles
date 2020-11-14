namespace CinelAirMiles.Common.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IGenericRepository<T> where T : class
    {

        /// <summary>
        /// Returns all instances of T entity
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();


        /// <summary>
        /// Returns a T entity by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(int id);


        /// <summary>
        /// Creates a new T entity in the context
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task CreateAsync(T entity);


        /// <summary>
        /// Updates a T entity in the context
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(T entity);


        /// <summary>
        /// Deletes a T entity from the context
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task DeleteAsync(T entity);


        /// <summary>
        /// Returns a true if the T entity with the respective ID is found the context
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(int id);
    }
}
