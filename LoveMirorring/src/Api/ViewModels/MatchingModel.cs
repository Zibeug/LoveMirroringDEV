﻿/*
 * Auteur : Sébastien Berger
 * Date : 07.05.2020
 * Détail : Modèle pour l'affichage d'un match
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    
    public class MatchingModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public int Age { get; set; }
        public string Sexe { get; set; }
        public string Profil { get; set; }
        public string Religion { get; set; }
        public string HairSize { get; set; }
        public string HairColor { get; set; }
        public string Sexuality { get; set; }
        public string Corpulence { get; set; }
        public string Style { get; set; }
        public string MusicName { get; set; }
        public string ArtisteName { get; set; }
        public double PourcentageMatching { get; set; }
    }
}
