using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class PreferencesMusique
    {
        [Key]
        public short PreferenceId { get; set; }
        [Key]
        public short MusiqueId { get; set; }

        [ForeignKey(nameof(MusiqueId))]
        [InverseProperty("PreferencesMusiques")]
        public virtual Musique Musique { get; set; }
        [ForeignKey(nameof(PreferenceId))]
        [InverseProperty("PreferencesMusiques")]
        public virtual Preference Preference { get; set; }
    }
}
