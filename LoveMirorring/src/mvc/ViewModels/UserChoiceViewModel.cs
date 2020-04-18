using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.ViewModels
{
    public class UserChoiceViewModel
    {
        public string UserName { get; set; }
        public int Age { get; set; }
        public string Sexe { get; set; }
        public string Profil { get; set; }

        public string Religion { get; set; }
        public string Corpulence { get; set; }
    }
}
