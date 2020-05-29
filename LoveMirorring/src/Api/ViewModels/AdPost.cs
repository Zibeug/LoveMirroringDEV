using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ViewModels
{
    public class AdPost
    {
        public short Id { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public string file { get; set; }
        public string name { get; set; }
        public string fileName { get; set; }
        public string ContentDisposition { get; set; }
        public string Link { get; set; }
        public string ContentType { get; set; }
    }
}
