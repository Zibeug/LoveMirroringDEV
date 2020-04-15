using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    [Table("HairColor")]
    public partial class HairColor
    {
        public HairColor()
        {
            AspNetUsers = new HashSet<AspNetUser>();
            PreferenceHairColors = new HashSet<PreferenceHairColor>();
        }

        [Key]
        public short HairColorId { get; set; }
        [Required]
        [StringLength(32)]
        public string HairColorName { get; set; }

        [InverseProperty(nameof(AspNetUser.HairColor))]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        [InverseProperty(nameof(PreferenceHairColor.HairColor))]
        public virtual ICollection<PreferenceHairColor> PreferenceHairColors { get; set; }
    }
}
