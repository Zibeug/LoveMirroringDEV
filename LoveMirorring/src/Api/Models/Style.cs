using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class Style
    {
        public Style()
        {
            PreferencesStyles = new HashSet<PreferencesStyle>();
        }

        [Key]
        public short StyleId { get; set; }
        [Required]
        [StringLength(32)]
        public string StyleName { get; set; }

        [InverseProperty(nameof(PreferencesStyle.Style))]
        public virtual ICollection<PreferencesStyle> PreferencesStyles { get; set; }
    }
}
