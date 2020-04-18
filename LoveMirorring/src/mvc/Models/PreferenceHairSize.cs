using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class PreferenceHairSize
    {
        [Key]
        public short PreferenceId { get; set; }
        [Key]
        public short HairSizeId { get; set; }

        [ForeignKey(nameof(HairSizeId))]
        [InverseProperty("PreferenceHairSizes")]
        public virtual HairSize HairSize { get; set; }
        [ForeignKey(nameof(PreferenceId))]
        [InverseProperty("PreferenceHairSizes")]
        public virtual Preference Preference { get; set; }
    }
}
