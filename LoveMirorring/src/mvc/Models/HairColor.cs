using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class HairColor
    {
        public HairColor()
        {
            PreferencesHairColors = new HashSet<PreferencesHairColor>();
        }

        [Key]
        public short HairColorId { get; set; }
        [Required]
        [StringLength(32)]
        public string HairColorName { get; set; }

        [InverseProperty(nameof(PreferencesHairColor.HairColor))]
        public virtual ICollection<PreferencesHairColor> PreferencesHairColors { get; set; }
    }
}
