using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class ProfilsPreference
    {
        [Key]
        public short PreferenceId { get; set; }
        [Key]
        public short ProfilId { get; set; }

        [ForeignKey(nameof(PreferenceId))]
        [InverseProperty("ProfilsPreferences")]
        public virtual Preference Preference { get; set; }
        [ForeignKey(nameof(ProfilId))]
        [InverseProperty("ProfilsPreferences")]
        public virtual Profil Profil { get; set; }
    }
}
