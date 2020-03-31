using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class Religion
    {
        public Religion()
        {
            PreferencesReligions = new HashSet<PreferencesReligion>();
        }

        [Key]
        public short ReligionId { get; set; }
        [Required]
        [StringLength(32)]
        public string ReligionName { get; set; }

        [InverseProperty(nameof(PreferencesReligion.Religion))]
        public virtual ICollection<PreferencesReligion> PreferencesReligions { get; set; }
    }
}
