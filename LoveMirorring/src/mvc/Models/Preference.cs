using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class Preference
    {
        public Preference()
        {
            PreferenceCorpulences = new HashSet<PreferenceCorpulence>();
            PreferenceHairColors = new HashSet<PreferenceHairColor>();
            PreferenceHairSizes = new HashSet<PreferenceHairSize>();
            PreferenceMusics = new HashSet<PreferenceMusic>();
            PreferenceReligions = new HashSet<PreferenceReligion>();
            PreferenceStyles = new HashSet<PreferenceStyle>();
        }

        [Key]
        public short PreferenceId { get; set; }
        [Required]
        [StringLength(450)]
        public string Id { get; set; }
        public short SexualityId { get; set; }
        public short AgeMin { get; set; }
        public short AgeMax { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(AspNetUser.Preferences))]
        public virtual AspNetUser IdNavigation { get; set; }
        [ForeignKey(nameof(SexualityId))]
        [InverseProperty("Preferences")]
        public virtual Sexuality Sexuality { get; set; }
        [InverseProperty(nameof(PreferenceCorpulence.Preference))]
        public virtual ICollection<PreferenceCorpulence> PreferenceCorpulences { get; set; }
        [InverseProperty(nameof(PreferenceHairColor.Preference))]
        public virtual ICollection<PreferenceHairColor> PreferenceHairColors { get; set; }
        [InverseProperty(nameof(PreferenceHairSize.Preference))]
        public virtual ICollection<PreferenceHairSize> PreferenceHairSizes { get; set; }
        [InverseProperty(nameof(PreferenceMusic.Preference))]
        public virtual ICollection<PreferenceMusic> PreferenceMusics { get; set; }
        [InverseProperty(nameof(PreferenceReligion.Preference))]
        public virtual ICollection<PreferenceReligion> PreferenceReligions { get; set; }
        [InverseProperty(nameof(PreferenceStyle.Preference))]
        public virtual ICollection<PreferenceStyle> PreferenceStyles { get; set; }
    }
}
