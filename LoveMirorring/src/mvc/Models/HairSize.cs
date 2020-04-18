using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class HairSize
    {
        public HairSize()
        {
            AspNetUsers = new HashSet<AspNetUser>();
            PreferenceHairSizes = new HashSet<PreferenceHairSize>();
        }

        [Key]
        public short HairSizeId { get; set; }
        [Required]
        [StringLength(32)]
        public string HairSizeName { get; set; }

        [InverseProperty(nameof(AspNetUser.HairSize))]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        [InverseProperty(nameof(PreferenceHairSize.HairSize))]
        public virtual ICollection<PreferenceHairSize> PreferenceHairSizes { get; set; }
    }
}
