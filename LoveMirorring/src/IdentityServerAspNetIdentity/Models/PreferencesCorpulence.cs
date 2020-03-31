using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class PreferencesCorpulence
    {
        [Key]
        public short PreferenceId { get; set; }
        [Key]
        public short CorpulenceId { get; set; }

        [ForeignKey(nameof(CorpulenceId))]
        [InverseProperty("PreferencesCorpulences")]
        public virtual Corpulence Corpulence { get; set; }
        [ForeignKey(nameof(PreferenceId))]
        [InverseProperty("PreferencesCorpulences")]
        public virtual Preference Preference { get; set; }
    }
}
