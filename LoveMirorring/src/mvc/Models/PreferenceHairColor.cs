using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class PreferenceHairColor
    {
        [Key]
        public short PreferenceId { get; set; }
        [Key]
        public short HairColorId { get; set; }

        [ForeignKey(nameof(HairColorId))]
        [InverseProperty("PreferenceHairColors")]
        public virtual HairColor HairColor { get; set; }
        [ForeignKey(nameof(PreferenceId))]
        [InverseProperty("PreferenceHairColors")]
        public virtual Preference Preference { get; set; }
    }
}
