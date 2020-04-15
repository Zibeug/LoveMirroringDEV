using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.ViewModels
{
    public class MatchingModel
    {
        [Required]
        public string Sexe { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Corpulence { get; set; }
        [Required]
        public string Religion { get; set; }
        [Required]
        public string Profil { get; set; }
    }
}
