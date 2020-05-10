using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.ViewModels
{
    public class SpotifyAlbum
    {
        private string album_group { get; set; }
        private string album_type { get; set; }
        private SpotifyArtist artists { get; set; }
        private string available_markets { get; set; }
        private SpotifyExternalUrl external_urls { get; set; }
        private string href { get; set; }
        private string id { get; set; }
        private List<SpotifyImage> images { get; set; }
        private string name { get; set; }
        private string release_date { get; set; }
        private string release_date_precision { get; set; }
        private string type { get; set; }
        private string uri { get; set; }
    }
}
