using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class PreferencesReligion
    {
        [Key]
        public short PreferenceId { get; set; }
        [Key]
        public short ReligionId { get; set; }

        [ForeignKey(nameof(PreferenceId))]
        [InverseProperty("PreferencesReligions")]
        public virtual Preference Preference { get; set; }
        [ForeignKey(nameof(ReligionId))]
        [InverseProperty("PreferencesReligions")]
        public virtual Religion Religion { get; set; }
    }
}
