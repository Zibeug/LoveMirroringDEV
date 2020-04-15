using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class PreferenceCorpulence
    {
        [Key]
        public short PreferenceId { get; set; }
        [Key]
        public short CorpulenceId { get; set; }

        [ForeignKey(nameof(CorpulenceId))]
        [InverseProperty("PreferenceCorpulences")]
        public virtual Corpulence Corpulence { get; set; }
        [ForeignKey(nameof(PreferenceId))]
        [InverseProperty("PreferenceCorpulences")]
        public virtual Preference Preference { get; set; }
    }
}
