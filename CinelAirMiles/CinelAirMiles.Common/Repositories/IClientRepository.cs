namespace CinelAirMiles.Common.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Entities;

    public interface IClientRepository : IGenericRepository<Client>
    {
        List<Client> GetClientsWithUsers();

        Task<Client> GetClientWithDetailsAsync(int? id);

        Task CreateClientWithUserAsync(User user, DateTime birthDate);

        /// <summary>
        /// Returns a client from the context that matches the supplied ProgramNumber
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<Client> GetClientByNumberAsync(string number);

        /// <summary>
        /// Returns a client from the context that matches the supplied user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<Client> GetClientByUserAsync(User user);

        Task<Client> GetClientByEmailAsync(string username);

        /// <summary>
        /// Edits a client, and if applicable, requests a tier change to be confirmed by a SuperUser. The logged user's username is required
        /// </summary>
        /// <param name="client"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> EditClientAsync(Client client, User user);
    }
}
