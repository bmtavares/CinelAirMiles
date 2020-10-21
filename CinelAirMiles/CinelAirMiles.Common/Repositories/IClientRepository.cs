namespace CinelAirMiles.Common.Repositories
{
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Entities;

    public interface IClientRepository : IGenericRepository<Client>
    {
        Task CreateClientWithUser(User user);

        Task<Client> GetClientByNumber(string number);
    }
}
