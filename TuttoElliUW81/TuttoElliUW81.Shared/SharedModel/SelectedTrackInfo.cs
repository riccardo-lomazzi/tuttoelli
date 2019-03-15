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
    public partial class SelectedTrackInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Track> array_tracce_currentAlbum;
        public ObservableCollection <Track> Array_tracce_currentAlbum {
            get {
                return array_tracce_currentAlbum;
            }
            set
            {
                if (value != this.array_tracce_currentAlbum)
                {
                    this.array_tracce_currentAlbum = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public Album current_album { get; set; }
        public int idTracciaScelta { get; set; }

        public SelectedTrackInfo()
        {
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
