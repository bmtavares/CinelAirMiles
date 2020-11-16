using CinelAirMiles.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CinelAirMiles.Common.Repositories
{
    public interface IComplaintRepository : IGenericRepository<Complaint>
    {
        /// <summary>
        /// Creates a new complains associated with the received user
        /// </summary>
        /// <param name="newComplaint"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> CreateComplaintAsync(Complaint newComplaint, User user);

        /// <summary>
        /// Returns complaints count.
        /// </summary>
        /// <returns></returns>
        Task<int> GetComplaintsCountAsync();

        /// <summary>
        /// Returns complaints by user
        /// </summary>
        /// <param name="milesProgramNumber"></param>
        /// <returns></returns>
        Task<List<Complaint>> GetComplaintAssociatedWithUserAsync(string milesProgramNumber);
   
    }
}
