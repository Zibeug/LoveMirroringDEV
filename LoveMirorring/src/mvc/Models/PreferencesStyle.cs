using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class PreferencesStyle
    {
        [Key]
        public short PreferenceId { get; set; }
        [Key]
        public short StyleId { get; set; }

        [ForeignKey(nameof(PreferenceId))]
        [InverseProperty("PreferencesStyles")]
        public virtual Preference Preference { get; set; }
        [ForeignKey(nameof(StyleId))]
        [InverseProperty("PreferencesStyles")]
        public virtual Style Style { get; set; }
    }
}
