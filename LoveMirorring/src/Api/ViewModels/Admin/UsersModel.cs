/*
 *      Auteur : Hans Morsch
 *      11.05.2020
 *      Model pour l'Api
 *      Permet de récuper un utilisisateur et ses rôles
 */

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
        public IEnumerable<AspNetRole> Roles { get; set; }
        public string UserId { get; set; }
    }
}
