/*
 *      Auteur : Hans Morsch
 *      11.05.2020
 *      Model pour l'Api
 *      Permet de compter le nombres de comptes créés
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.ViewModels.Admin
{
    public class IndexModel
    {
        public int nbUsers { get; set; }
        public decimal earningsMonthly { get; set; }
        public decimal earningsAnnualy { get; set; }
        public int nbConnexion { get; set; }
    }
}
