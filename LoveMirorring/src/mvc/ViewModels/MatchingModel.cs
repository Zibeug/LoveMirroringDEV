using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.ViewModels
{
    public class MatchingModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
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
        public string HairSize { get; set; }
        public string HairColor { get; set; }
        public string Sexuality { get; set; }
        public string Style { get; set; }
    }
}
