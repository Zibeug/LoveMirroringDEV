using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class Sexuality
    {
        public Sexuality()
        {
            AspNetUsers = new HashSet<AspNetUser>();
            Preferences = new HashSet<Preference>();
        }

        [Key]
        public short SexualityId { get; set; }
        [Required]
        [StringLength(32)]
        public string SexualityName { get; set; }

        [InverseProperty(nameof(AspNetUser.Sexuality))]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        [InverseProperty(nameof(Preference.Sexuality))]
        public virtual ICollection<Preference> Preferences { get; set; }
    }
}
