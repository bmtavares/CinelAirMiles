using CinelAirMiles.Web.Backoffice.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Data.Repositories.Interfaces
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task CreateClientWithUser(User user);
    }
}
