using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class HairSize
    {
        public HairSize()
        {
            PreferencesHairSizes = new HashSet<PreferencesHairSize>();
        }

        [Key]
        public short HairSizeId { get; set; }
        [Required]
        [StringLength(32)]
        public string HairSizeName { get; set; }

        [InverseProperty(nameof(PreferencesHairSize.HairSize))]
        public virtual ICollection<PreferencesHairSize> PreferencesHairSizes { get; set; }
    }
}
