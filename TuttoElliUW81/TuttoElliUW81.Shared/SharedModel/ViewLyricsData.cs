using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TuttoElliUW81.SharedModel
{
    public class ViewLyricsData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private SelectedTrackInfo trcInfo;
        public SelectedTrackInfo TrcInfo {
            get
            {
                return trcInfo;
            }
            set
            {
                if(value!=this.trcInfo)
                {
                    this.trcInfo = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public ObservableCollection<Track> tracks { get; set; }
        public ObservableCollection<Album> albums{ get; set; }

        public ViewLyricsData()
        {
            trcInfo = new SelectedTrackInfo();
            tracks = new ObservableCollection<Track>();
            albums = new ObservableCollection<Album>();
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
