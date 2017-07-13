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
        [Display(Name ="Episode")]
        public int Number { get { return _number; } set { _number=value; } }
        public string Added { get { return _added; } set { _added = value; } }
        public string ImgURL { get { return _imgURL; } set { _imgURL = value; } }

        private int _id;
        private string _anime;
        private int _number;
        private string _added;
        private string _imgURL;

        public Episode(string anime, int number, string added, string imgURL)
        {
            _id = nextID;
            nextID += 1;
            _anime = anime;
            _number = number;
            _added = added;
            _imgURL = imgURL;   
        }
    }
}
