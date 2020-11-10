namespace CinelAirMiles.Common.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Entities;

    public interface IClientRepository : IGenericRepository<Client>
    {
        List<Client> GetClientsWithUsers();

        Task<Client> GetClientWithDetailsAsync(int? id);

        Task CreateClientWithUserAsync(User user, DateTime birthDate);

        Task<Client> GetClientByNumberAsync(string number);

        Task<Client> GetClientByUserAsync(User user);

        /// <summary>
        /// Edits a client, and if applicable, requests a tier change to be confirmed by a SuperUser. The logged user's username is required
        /// </summary>
        /// <param name="client"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task EditClientAsync(Client client, User user);
    }
}
