using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyFirstProject.Models;
using System.Xml.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Xml;

namespace MyFirstProject.Logic
{
    public static class Connector
    {
        private static long lastUpdate = DateTime.Now.Millisecond;
        private static string baseURL = "http://anilinkz.to/";

        private static Episode getEpisode(XElement episodeElement)
        {
            //Episode e = _context.Episode.SingleOrDefault(ep => ep.ID == id);

            //if (e != null)
            //     return e;

            //XElement name = (from el in node.ElementsAfterSelf())
            //e = new Episode("anime",1,"today");
 
            string anime = episodeElement.Descendants().Where(x => (string)x.Attribute("class") == "ser").FirstOrDefault().Attribute("title").Value;
            int episode = Convert.ToInt16((episodeElement.Descendants().Where(x => (string)x.Attribute("class") == "title").FirstOrDefault().Value.Replace("Episode ","")));
            string added = "Today";
            string imgURL = episodeElement.Descendants().Where(x => (string)x.Attribute("class") == "img").FirstOrDefault().Attribute("style").Value;
            imgURL = imgURL.Substring(imgURL.IndexOf("(") + 1);
            imgURL = imgURL.Substring(0, imgURL.IndexOf(")"));
            return new Episode(anime, episode, added,baseURL+imgURL);  
        }

        public static List<Episode> getTrendingEpisodes()
        {

            byte[] data = Encoding.ASCII.GetBytes($"action=ongoing&page=1&type=0");
            WebRequest request = WebRequest.Create(baseURL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            
            using (Stream stream = request.GetRequestStreamAsync().Result)
            {
                stream.Write(data, 0, data.Length);
            }

            string responseContent = null;

            using (WebResponse response =  request.GetResponseAsync().Result)
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader sr99 = new StreamReader(stream))
                    {
                        responseContent = sr99.ReadToEnd();
                    }
                }
            }

            List<Episode> list = new List<Episode>();
            responseContent = responseContent.Substring(responseContent.IndexOf("<ul id=\"trendinglist\""));
            responseContent = responseContent.Substring(0, responseContent.IndexOf("</ul>") + 5);
            XDocument doc = XDocument.Parse(responseContent);
            foreach (XNode node in doc.Elements().Descendants("li"))
            {
                list.Add(getEpisode((XElement)node));
            }
            return list;
        }

    }
}
