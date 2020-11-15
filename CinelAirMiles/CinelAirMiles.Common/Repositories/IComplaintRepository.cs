using CinelAirMiles.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CinelAirMiles.Common.Repositories
{
    public interface IComplaintRepository : IGenericRepository<Complaint>
    {
        Task<string> CreateComplaintAsync(Complaint newComplaint, User user);

        Task<int> GetComplaintsCountAsync();

        Task<List<Complaint>> GetComplaintAssociatedWithUserAsync(string milesProgramNumber);
   
    }
}
