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
            PreferenceStyles = new HashSet<PreferenceStyle>();
            UserStyles = new HashSet<UserStyle>();
        }

        [Key]
        public short StyleId { get; set; }
        [Required]
        [StringLength(32)]
        public string StyleName { get; set; }

        [InverseProperty(nameof(PreferenceStyle.Style))]
        public virtual ICollection<PreferenceStyle> PreferenceStyles { get; set; }
        [InverseProperty(nameof(UserStyle.Style))]
        public virtual ICollection<UserStyle> UserStyles { get; set; }
    }
}
