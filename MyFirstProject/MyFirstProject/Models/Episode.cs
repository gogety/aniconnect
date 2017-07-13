using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyFirstProject.Models
{
    public class Episode
    {
        private static int nextID = 0;
        public int ID { get { return _id; } set { _id = value; } }
        public string Anime { get { return _anime; } set { _anime = value; } }
        [Display(Name ="Added")]
      //  public int Number { get { return _number; } set { _number=value; } }
        public string Added { get { return _added; } set { _added = value; } }
        public string ImgURL { get { return _imgURL; } set { _imgURL = value; } }
        public string Title { get { return _title; } set { _title = value; } }
        public string DetailsURL { get { return _detailsURL; } set { _detailsURL = value; } }

        private int _id;
        private string _anime;
        //private int _number;
        private string _title;
        private string _added;
        private string _imgURL;
        private string _detailsURL;

        public Episode(string anime, string title, string added, string imgURL, string detailsURL)
        {
            _id = nextID;
            nextID += 1;
            _anime = anime;
            _title = title;
            _added = added;
            _imgURL = imgURL;
            _detailsURL = detailsURL;
        }
    }
}
