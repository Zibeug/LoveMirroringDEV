using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.ViewModels
{
    public class SpotifyInput
    {
        // Représente ses préférences en musique
        public string favoriteCategory { get; set; }

        // Représente ce que son potentiel match poourrait écouter commme style de musique
        public string likeCategory { get; set; }
        public string Song { get; set; }
    }
}
