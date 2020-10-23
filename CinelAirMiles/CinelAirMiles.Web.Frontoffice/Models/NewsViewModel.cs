using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CinelAirMiles.Web.Frontoffice.Models
{
    [Serializable, XmlRoot(ElementName = "rss")]
    public class NewsViewModel
    {
        [XmlElement("channel")]
        public NewsChannelViewModel Channel { get; set; }
    }
}
