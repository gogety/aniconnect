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

        private static Episode createEpisode(XElement episodeElement)
        {
            //TODO : check db first ?
            string anime = episodeElement.Descendants().Where(x => (string)x.Attribute("class") == "ser").FirstOrDefault().Attribute("title").Value;
            string title = episodeElement.Descendants().Where(x => (string)x.Attribute("class") == "title").FirstOrDefault().Value;
            string added = string.Empty;
            try
            {
                added = episodeElement.Descendants("small").FirstOrDefault().Value;
            }
            catch{
                //default value, as added is not always present
                added = "Today";
            };
            string imgURL = episodeElement.Descendants().Where(x => (string)x.Attribute("class") == "img").FirstOrDefault().Attribute("style").Value;
            imgURL = imgURL.Substring(imgURL.IndexOf("(") + 1);
            imgURL = imgURL.Substring(0, imgURL.IndexOf(")"));
            string detailsURL = episodeElement.Descendants().Where(x => (string)x.Attribute("class") == "ep").FirstOrDefault().Attribute("href").Value;
            return new Episode(anime, title, added,baseURL+imgURL, detailsURL);  
        }

        private static string executeWebrequest(string url, string parameters = "")
        {
            WebRequest request = WebRequest.Create(baseURL + url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (parameters.Length > 0)
            {

                byte[] data = Encoding.ASCII.GetBytes(parameters);
                using (Stream stream = request.GetRequestStreamAsync().Result)
                {
                    stream.Write(data, 0, data.Length);
                }

            }

            string responseContent = null;

            using (WebResponse response = request.GetResponseAsync().Result)
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader sr99 = new StreamReader(stream))
                    {
                        responseContent = sr99.ReadToEnd();
                    }
                }
            }

            return responseContent;
        }

        public static List<Episode> getTrendingEpisodes()
        {
            //TODO : add to db instead of loading all the time (default SQLite, inMemory, PostGreSQL? ...)
            //Content is enough to fill a light model, not the full one with the actual video link

            List<Episode> list = new List<Episode>();
            string webContent = executeWebrequest("");

            webContent = webContent.Substring(webContent.IndexOf("<ul id=\"trendinglist\""));
            webContent = webContent.Substring(0, webContent.IndexOf("</ul>") + 5);
            XDocument doc = XDocument.Parse(webContent);
            foreach (XNode node in doc.Elements().Descendants("li"))
            {
                list.Add(createEpisode((XElement)node));
            }

            return list;
        }

        public static List<Episode> getLatestEpisodes(int page = 1)
        {
            //TODO : add to db instead of loading all the time (default SQLite, inMemory, PostGreSQL? ...)
            //Content is enough to fill a light model, not the full one with the actual video link

            List<Episode> list = new List<Episode>();
            string parameters = $"action=ongoing&page={page}&type=0";
            //need one main tag to be parseable in xml
            string webContent = "<root>" + executeWebrequest("fetch.php", parameters) + "</root>"; 

            XDocument doc = XDocument.Parse(webContent);
            foreach (XNode node in doc.Elements().Descendants("li"))
            {
                list.Add(createEpisode((XElement)node));
            }

            return list;
        }

        public static string getVideoLink(string detailsURL)
        {
            string link = string.Empty;
            string webContent = executeWebrequest(detailsURL);
            webContent = webContent.Substring(webContent.IndexOf("<div id=\"player\">"));
            webContent = webContent.Substring(0, webContent.IndexOf("</div><div id=\"playinfo\"") + 7);
            XDocument doc = XDocument.Parse(webContent);
            XElement playerDiv = (XElement)doc.Descendants("div").Where(x => (string)x.Attribute("id") == "player").FirstOrDefault();
            //link is either in an iframe or in a video tag
            if (playerDiv.Descendants("iframe").Count() > 0)
            {
                playerDiv = playerDiv.Descendants("iframe").FirstOrDefault();
            }
            else
            {
                playerDiv = playerDiv.Descendants("video").FirstOrDefault();
            }
            link = playerDiv.Attribute("src").Value;
                
            return link;
        }

       

    }
}
