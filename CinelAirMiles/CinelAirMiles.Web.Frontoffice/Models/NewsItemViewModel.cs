using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CinelAirMiles.Web.Frontoffice.Models
{
    public class NewsItemViewModel
    {
        [XmlElement("title")]
        public string Title { get; set; }


        [XmlElement("link")]
        public string Link { get; set; }


        [XmlElement("description")]
        public string Description { get; set; }

        [Display(Name = "Publication date")]
        [XmlElement("pubDate")]
        public string PubDate { get; set; }


        [XmlElement("author")]
        public string Author { get; set; }
    }
}
