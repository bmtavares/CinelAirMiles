namespace CinelAirMiles.Common.Repositories
{
    using CinelAirMiles.Common.Entities;

    using System.Threading.Tasks;

    public interface IContactFormRepository : IGenericRepository<ContactForm>
    {
        /// <summary>
        /// Returns contact forms count
        /// </summary>
        /// <returns>Contact forms count</returns>
        Task<int> GetCountAsync();
    }
}
