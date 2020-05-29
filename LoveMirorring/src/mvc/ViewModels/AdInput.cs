using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.ViewModels
{
    public class AdInput
    {
        public short Id { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public IFormFile file { get; set; }
    }
}
