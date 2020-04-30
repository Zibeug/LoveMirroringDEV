using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class PreferenceStyle
    {
        [Key]
        public short PreferenceId { get; set; }
        [Key]
        public short StyleId { get; set; }

        [ForeignKey(nameof(PreferenceId))]
        [InverseProperty("PreferenceStyles")]
        public virtual Preference Preference { get; set; }
        [ForeignKey(nameof(StyleId))]
        [InverseProperty("PreferenceStyles")]
        public virtual Style Style { get; set; }
    }
}
