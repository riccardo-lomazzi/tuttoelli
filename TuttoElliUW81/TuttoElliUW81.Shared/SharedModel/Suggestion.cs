using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuttoElliUW81.SharedModel
{
    public class Suggestion
    {

        private int idalbum;
        private string album_name;
        private int idtrack;
        private string track_title;
        private string album_img;



        public string album_img_src
        {
            get
            {
                return @"/Assets/Images/" + Album_img;
            }
        }

        public int Idalbum
        {
            get
            {
                return idalbum;
            }

            set
            {
                idalbum = value;
            }
        }

        public int Idalbum_forArray
        {
            get
            {
                return idalbum - 1;
            }
        }

        public string Album_name
        {
            get
            {
                return album_name;
            }

            set
            {
                album_name = value;
            }
        }

        public int Idtrack
        {
            get
            {
                return idtrack;
            }

            set
            {
                idtrack = value;
            }
        }

        public int Idtrack_forArray
        {
            get
            {
                return idtrack - 1;
            }
        }

        public string Track_title
        {
            get
            {
                return track_title;
            }

            set
            {
                track_title = value;
            }
        }

        public string Album_img
        {
            get
            {
                return album_img;
            }

            set
            {
                album_img = value;
            }
        }

        public Suggestion(string album_name, string track_title, string album_img, int idtrack)
        {
            this.Album_name = album_name;
            this.Track_title = track_title;
            this.Album_img = album_img;
            this.Idtrack = idtrack;
        }
    }
}
