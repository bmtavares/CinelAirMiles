using CinelAirMiles.Web.Frontoffice.Helpers.Interfaces;
using CinelAirMiles.Web.Frontoffice.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace CinelAirMiles.Web.Frontoffice.Helpers.Classes
{
    public class XmlHelper : IXmlHelper
    {
        public NewsViewModel DeserializeNewsXml(string url)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(NewsViewModel));

            WebClient client = new WebClient();

            string data = Encoding.Default.GetString(client.DownloadData(url));

            Stream fileStream = new MemoryStream(Encoding.UTF8.GetBytes(data));
            
            var news = (NewsViewModel) serializer.Deserialize(fileStream);

            foreach (var item in news.Channel.Item)
            {
                var substring = item.Author.Split("¦");

                item.Author = substring[1];
            }

            return news;
        }
    }
}
