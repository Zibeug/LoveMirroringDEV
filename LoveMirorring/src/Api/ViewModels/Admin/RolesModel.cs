/*
 *      Auteur : Hans Morsch
 *      11.05.2020
 *      Model pour l'Api
 *      Permet de récupérer la liste des roles et des utilisateurs selon un autre model
 */

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
