using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TuttoElliUW81.SharedModel
{
    public class Track : INotifyPropertyChanged
    {
        private int idtrack;
        private int idalbum;
        private string track_title;
        private string track_text;
        private string durata;

        public int Idtrack {
            get
            {
                return idtrack;
            }
            set
            {
                if (value != this.idtrack)
                {
                    this.idtrack = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int Idtrack_forArray
        {
            get
            {
                return idtrack - 1;
            }
            
        }
        public int Idalbum {
            get
            {
                return idalbum;
            }
            set
            {
                if (value != this.idalbum)
                {
                    this.idalbum = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Track_title {
            get
            {
                return track_title;
            }
            set
            {
                if (value != this.track_title)
                {
                    this.track_title = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string track_title_toUpper
        {
            get
            {
                return track_title.ToUpper();
            }
        }

        public string Track_text
        {
            get
            {
                return track_text;
            }
            set
            {
                if (value != this.track_text)
                {
                    this.track_text = value;
                    NotifyPropertyChanged();
                }
            }
        }

        
        public string Track_length
        {
            get
            {
                return durata;
            }
            set
            {
                if ((value != this.durata) || (value!=null))
                {
                    this.durata = value;
                    NotifyPropertyChanged();
                }
                else
                {
                    durata = "";
                }
            }

        }
        
        public Track()
        {
            //empty constructor
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
