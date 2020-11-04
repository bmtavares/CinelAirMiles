namespace CinelAirMiles.Common.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Entities;

    public interface IClientRepository : IGenericRepository<Client>
    {
        List<Client> GetClientsWithUsers();

        Task<Client> GetClientWithDetailsAsync(int? id);

        Task CreateClientWithUserAsync(User user);

        Task<Client> GetClientByNumber(string number);
    }
}
