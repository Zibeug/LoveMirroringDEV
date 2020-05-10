using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ViewModels.Admin
{
    public class RolesModel
    {
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<UsersModel> Users { get; set; }

    }
}
