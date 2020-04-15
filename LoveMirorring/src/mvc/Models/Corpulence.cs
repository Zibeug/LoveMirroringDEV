using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class Corpulence
    {
        public Corpulence()
        {
            AspNetUsers = new HashSet<AspNetUser>();
            PreferenceCorpulences = new HashSet<PreferenceCorpulence>();
        }

        [Key]
        public short CorpulenceId { get; set; }
        [Required]
        [StringLength(32)]
        public string CorpulenceName { get; set; }

        [InverseProperty(nameof(AspNetUser.Corpulence))]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        [InverseProperty(nameof(PreferenceCorpulence.Corpulence))]
        public virtual ICollection<PreferenceCorpulence> PreferenceCorpulences { get; set; }
    }
}
