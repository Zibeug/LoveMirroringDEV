using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ViewModels.Admin
{
    public class UsersModel
    {
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string UserId { get; set; }
    }
}
