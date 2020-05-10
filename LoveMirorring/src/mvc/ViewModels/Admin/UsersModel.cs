using mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.ViewModels.Admin
{
    public class UsersModel
    {
        public string Email { get; set; }
        public IEnumerable<AspNetRole> Roles { get; set; }
        public string UserId { get; set; }
    }
}
