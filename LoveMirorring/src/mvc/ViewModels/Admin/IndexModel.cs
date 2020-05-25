/*
 *      Auteur : Hans Morsch
 *      11.05.2020
 *      Model pour le MVC
 *      Permet de compter le nombres de comptes créés
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.ViewModels.Admin
{
    public class IndexModel
    {
        public int nbUsers { get; set; }
        public decimal earningsMonthly { get; set; }
        public decimal earningsAnnualy { get; set; }
        public int nbConnexion { get; set; }
    }
}
