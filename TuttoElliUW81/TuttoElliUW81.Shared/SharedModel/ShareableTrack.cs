using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuttoElliUW81.SharedModel
{
    public class ShareableTrack
    {
        private string TrackLyrics;
        private string TrackTitle;
        private string AlbumTitle;

        public string PublicTrackLyrics
        {
            get
            {
                return TrackLyrics;
            }
            set
            {
                TrackLyrics = value;
            }
        }

        public string PublicTrackTitle
        {
            get
            {
                return TrackTitle;
            }
            set
            {
                TrackTitle = value;
            }
        }
        public string PublicAlbumTitle
        {
            get
            {
                return AlbumTitle;
            }
            set
            {
                AlbumTitle = value;
            }
        }

    }
}
