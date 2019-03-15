using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuttoElliUW81.SharedModel
{
    public class Album
    {

        private int idalbum;
        private string album_name;
        private string album_year;
        private string album_label;
        private string album_type;
        private string album_img;
        private string album_info;
        private string marok_link;

        public int Idalbum
        {
            get
            {
                return idalbum;
            }
            set
            {
                this.idalbum = value;
            }
        }

        public int idalbum_forArray
        {
            get
            {
                return idalbum - 1;
            }
        }

        public string album_name_toUpper
        {
            get
            {
                return Album_name.ToUpper();
            }
        }

        public string album_img_src
        {
            get
            {
                return @"/Assets/Images/" + Album_img;
            }
        }

        public string Album_year
        {
            get
            {
                return album_year;
            }

            set
            {
               album_year = value;
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


        public string Album_label
        {
            get
            {
                return album_label;
            }

            set
            {
                album_label = value;
            }
        }

        public string Album_type
        {
            get
            {
                return album_type;
            }

            set
            {
                album_type = value;
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

        public string Album_info
        {
            get
            {
                return album_info;
            }

            set
            {
                album_info = value;
            }
        }

        public string Marok_link
        {
            get
            {
                return marok_link;
            }
            set
            {
                marok_link = value;
            }
        }

        public Album()
        {
            this.idalbum = 0;
            this.Album_info = null;
            this.Album_label = null;
            this.Album_name = null;
            this.Album_type = null;
            this.Album_year = null;
            this.Album_img=null;
        }




        public Album(int idalbum, string album_name, string album_year, string alb_lab, string tipoAlb, string imgAlbumPath, string albumInfo)
        {
            this.idalbum = idalbum;
            this.Album_name = album_name;
            this.Album_year = album_year;
            this.Album_label = alb_lab;
            this.Album_type = tipoAlb;
            this.Album_img = imgAlbumPath;
            this.Album_info = albumInfo;
        }
    }
}
