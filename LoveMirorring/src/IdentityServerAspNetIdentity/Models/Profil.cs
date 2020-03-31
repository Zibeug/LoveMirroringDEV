using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class Profil
    {
        public Profil()
        {
            Answers = new HashSet<Answer>();
            ProfilsPreferences = new HashSet<ProfilsPreference>();
            UsersProfils = new HashSet<UsersProfil>();
        }

        [Key]
        public short ProfilId { get; set; }
        [Required]
        [StringLength(64)]
        public string ProfilName { get; set; }
        [Required]
        [StringLength(255)]
        public string ProfilDescription { get; set; }
        public short ProfilValue { get; set; }

        [InverseProperty(nameof(Answer.Profil))]
        public virtual ICollection<Answer> Answers { get; set; }
        [InverseProperty(nameof(ProfilsPreference.Profil))]
        public virtual ICollection<ProfilsPreference> ProfilsPreferences { get; set; }
        [InverseProperty(nameof(UsersProfil.Profil))]
        public virtual ICollection<UsersProfil> UsersProfils { get; set; }
    }
}
