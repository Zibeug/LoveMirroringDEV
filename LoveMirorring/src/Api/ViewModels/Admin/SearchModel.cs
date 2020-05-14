/*
 *      Auteur : Hans Morsch
 *      11.05.2020
 *      Model pour l'Api
 *      Permet de récupérer des informations d'un utilisateur
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ViewModels.Admin
{
    public class SearchModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool QuizCompleted { get; set; }
    }
}
