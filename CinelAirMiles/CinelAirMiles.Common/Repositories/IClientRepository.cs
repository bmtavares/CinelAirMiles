namespace CinelAirMiles.Common.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Entities;

    public interface IClientRepository : IGenericRepository<Client>
    {
        //Task ChangeClientTierAutomatically(Client client, ProgramTier tier);

        /// <summary>
        /// Returns all clients and their respective users from context
        /// </summary>
        /// <returns></returns>
        List<Client> GetClientsWithUsers();

        /// <summary>
        /// Returns a client from its ID, with all associated details included from context
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Client> GetClientWithDetailsAsync(int? id);

        /// <summary>
        /// Creates a new client with a randomly generated client number, associated with the received user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="birthDate"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a clients with the matching e-mail from the context
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
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
