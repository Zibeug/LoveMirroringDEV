using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class PreferencesHairColor
    {
        [Key]
        public short PreferenceId { get; set; }
        [Key]
        public short HairColorId { get; set; }

        [ForeignKey(nameof(HairColorId))]
        [InverseProperty("PreferencesHairColors")]
        public virtual HairColor HairColor { get; set; }
        [ForeignKey(nameof(PreferenceId))]
        [InverseProperty("PreferencesHairColors")]
        public virtual Preference Preference { get; set; }
    }
}
