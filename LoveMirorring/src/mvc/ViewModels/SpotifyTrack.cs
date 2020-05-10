using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.ViewModels
{
    public class SpotifyTrack
    {
        private SpotifyAlbum album { get; set; }
        private SpotifyArtist artists { get; set; }

        private string[] available_markets { get; set; }
        private int disc_number { get; set; }
        private int duration_ms { get; set; }
        private bool Explicit { get; set; }
        private SpotifyExternalID external_ids { get; set; }
        private SpotifyExternalUrl external_urls { get; set; }
        private string href { get; set; }
        private string id { get; set; }
        private bool is_playable { get; set; }
        private SpotifyTrackLink linked_from { get; set; }
        private string name { get; set; }
        private int popularity { get; set; }
        private string preview_url { get; set; }
        private int track_number { get; set; }
        private string type { get; set; }
        private string uri { get; set; }
    }
}
