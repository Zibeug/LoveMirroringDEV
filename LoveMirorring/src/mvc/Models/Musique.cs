using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class Musique
    {
        public Musique()
        {
            PreferencesMusiques = new HashSet<PreferencesMusique>();
        }

        [Key]
        public short MusiqueId { get; set; }
        [Required]
        [StringLength(32)]
        public string MusiqueName { get; set; }

        [InverseProperty(nameof(PreferencesMusique.Musique))]
        public virtual ICollection<PreferencesMusique> PreferencesMusiques { get; set; }
    }
}
