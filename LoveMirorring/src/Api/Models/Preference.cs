using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class Preference
    {
        public Preference()
        {
            PreferencesCorpulences = new HashSet<PreferencesCorpulence>();
            PreferencesHairColors = new HashSet<PreferencesHairColor>();
            PreferencesHairSizes = new HashSet<PreferencesHairSize>();
            PreferencesMusiques = new HashSet<PreferencesMusique>();
            PreferencesReligions = new HashSet<PreferencesReligion>();
            PreferencesStyles = new HashSet<PreferencesStyle>();
            ProfilsPreferences = new HashSet<ProfilsPreference>();
            UsersPreferences = new HashSet<UsersPreference>();
        }

        [Key]
        public short PreferenceId { get; set; }
        public short SexualityId { get; set; }
        public bool Religions { get; set; }
        public bool Age { get; set; }
        public bool Corpulences { get; set; }

        [ForeignKey(nameof(SexualityId))]
        [InverseProperty("Preferences")]
        public virtual Sexuality Sexuality { get; set; }
        [InverseProperty(nameof(PreferencesCorpulence.Preference))]
        public virtual ICollection<PreferencesCorpulence> PreferencesCorpulences { get; set; }
        [InverseProperty(nameof(PreferencesHairColor.Preference))]
        public virtual ICollection<PreferencesHairColor> PreferencesHairColors { get; set; }
        [InverseProperty(nameof(PreferencesHairSize.Preference))]
        public virtual ICollection<PreferencesHairSize> PreferencesHairSizes { get; set; }
        [InverseProperty(nameof(PreferencesMusique.Preference))]
        public virtual ICollection<PreferencesMusique> PreferencesMusiques { get; set; }
        [InverseProperty(nameof(PreferencesReligion.Preference))]
        public virtual ICollection<PreferencesReligion> PreferencesReligions { get; set; }
        [InverseProperty(nameof(PreferencesStyle.Preference))]
        public virtual ICollection<PreferencesStyle> PreferencesStyles { get; set; }
        [InverseProperty(nameof(ProfilsPreference.Preference))]
        public virtual ICollection<ProfilsPreference> ProfilsPreferences { get; set; }
        [InverseProperty(nameof(UsersPreference.Preference))]
        public virtual ICollection<UsersPreference> UsersPreferences { get; set; }
    }
}
