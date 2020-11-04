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

            for (int i = 0; i < news.Channel.Item.Count; i++)
            {
                if (news.Channel.Item[i] != null)
                {
                    news.Channel.Item[i].Title = CheckTitle(news.Channel.Item[i].Title);
                    news.Channel.Item[i].Link = CheckLink(news.Channel.Item[i].Link);
                    news.Channel.Item[i].Description = CheckDecription(news.Channel.Item[i].Description);
                    news.Channel.Item[i].Author = CheckAuthor(news.Channel.Item[i].Author);
                    news.Channel.Item[i].PubDate = CheckPubDate(news.Channel.Item[i].PubDate);
                }
            }

            news.Channel.Item.RemoveAll(n => n == null);

            return news;
        }

        string CheckAuthor(string author)
        {
            if(author != null)
            {
                var substring = author.Split("¦");

                author = substring[1];

                return author;
            }

            return "No author...";
        }

        string CheckTitle(string title)
        {
            if (title != null)
            {
                return title;
            }

            return "No title...";
        }

        string CheckLink(string link)
        {
            if (link != null)
            {
                return link;
            }

            return "#";
        }

        string CheckDecription(string description)
        {
            if (description != null)
            {
                return description;
            }

            return "No description...";
        }

        string CheckPubDate(string pubDate)
        {
            if (pubDate != null)
            {
                return pubDate;
            }

            return "No publication date...";
        }


    }
}
