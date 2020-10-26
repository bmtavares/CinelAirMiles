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
        [XmlElement("title", IsNullable = true)]
        public string Title { get; set; }


        [XmlElement("link", IsNullable = true)]
        public string Link { get; set; }


        [XmlElement("description", IsNullable = true)]
        public string Description { get; set; }

        [Display(Name = "Publication date")]
        [XmlElement("pubDate", IsNullable = true)]
        public string PubDate { get; set; }


        [XmlElement("author", IsNullable = true)]
        public string Author { get; set; }
    }
}
