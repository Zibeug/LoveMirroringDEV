/*
 *      Auteur : Hans Morsch
 *      11.05.2020
 *      Model pour le MVC
 *      Permet de mettre à jour les roles d'un utilisateur
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.ViewModels.Admin
{
    public class UpdateUserRoleModel
    {
        public IEnumerable<UsersModel> Users { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string UserEmail { get; set; }
        public string Role { get; set; }
        public bool DeleteRole { get; set; }
    }
}
