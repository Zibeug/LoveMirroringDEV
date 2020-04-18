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
            PreferenceReligions = new HashSet<PreferenceReligion>();
        }

        [Key]
        public short ReligionId { get; set; }
        [Required]
        [StringLength(32)]
        public string ReligionName { get; set; }

        [InverseProperty(nameof(PreferenceReligion.Religion))]
        public virtual ICollection<PreferenceReligion> PreferenceReligions { get; set; }
    }
}
