using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class Corpulence
    {
        public Corpulence()
        {
            PreferencesCorpulences = new HashSet<PreferencesCorpulence>();
        }

        [Key]
        public short CorpulenceId { get; set; }
        [Required]
        [StringLength(32)]
        public string CorpulenceName { get; set; }

        [InverseProperty(nameof(PreferencesCorpulence.Corpulence))]
        public virtual ICollection<PreferencesCorpulence> PreferencesCorpulences { get; set; }
    }
}
