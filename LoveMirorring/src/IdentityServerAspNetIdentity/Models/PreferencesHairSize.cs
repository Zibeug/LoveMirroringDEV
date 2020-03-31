using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class PreferencesHairSize
    {
        [Key]
        public short PreferenceId { get; set; }
        [Key]
        public short HairSizeId { get; set; }

        [ForeignKey(nameof(HairSizeId))]
        [InverseProperty("PreferencesHairSizes")]
        public virtual HairSize HairSize { get; set; }
        [ForeignKey(nameof(PreferenceId))]
        [InverseProperty("PreferencesHairSizes")]
        public virtual Preference Preference { get; set; }
    }
}
