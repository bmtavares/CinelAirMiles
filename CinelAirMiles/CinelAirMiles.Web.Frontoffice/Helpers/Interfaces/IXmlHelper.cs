using CinelAirMiles.Web.Frontoffice.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CinelAirMiles.Web.Frontoffice.Helpers.Interfaces
{
    public interface IXmlHelper
    {
        NewsViewModel DeserializeNewsXml(string url);
    }
}
