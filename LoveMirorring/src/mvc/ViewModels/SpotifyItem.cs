using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.ViewModels
{
    public class SpotifyItem
    {
        public string href { get; set; }
        public List<SpotifyIcons> icons { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public SpotifyError error { get; set; }

    }
}