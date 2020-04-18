using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class PreferenceMusic
    {
        [Key]
        public short PreferenceId { get; set; }
        [Key]
        public short MusicId { get; set; }

        [ForeignKey(nameof(MusicId))]
        [InverseProperty("PreferenceMusics")]
        public virtual Music Music { get; set; }
        [ForeignKey(nameof(PreferenceId))]
        [InverseProperty("PreferenceMusics")]
        public virtual Preference Preference { get; set; }
    }
}
