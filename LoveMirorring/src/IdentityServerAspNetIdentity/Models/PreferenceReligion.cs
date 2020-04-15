using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class PreferenceReligion
    {
        [Key]
        public short PreferenceId { get; set; }
        [Key]
        public short ReligionId { get; set; }

        [ForeignKey(nameof(PreferenceId))]
        [InverseProperty("PreferenceReligions")]
        public virtual Preference Preference { get; set; }
        [ForeignKey(nameof(ReligionId))]
        [InverseProperty("PreferenceReligions")]
        public virtual Religion Religion { get; set; }
    }
}
