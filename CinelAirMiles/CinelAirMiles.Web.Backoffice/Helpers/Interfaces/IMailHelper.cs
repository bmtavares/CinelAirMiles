using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Backoffice.Helpers.Interfaces
{
    public interface IMailHelper
    {
        void SendMail(string to, string subject, string body);
    }
}
