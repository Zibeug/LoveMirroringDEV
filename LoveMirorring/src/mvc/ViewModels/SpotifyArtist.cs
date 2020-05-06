using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace mvc.ViewModels
{
    public class SpotifyArtist
    {
        private SpotifyExternalUrl external_urls { get; set; }
        private string href { get; set; }
        private string id { get; set; }
        private string name { get; set; }
        private string type { get; set; }
        private string uri { get; set; }
    }
}
