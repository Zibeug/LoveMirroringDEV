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
            UserProfils = new HashSet<UserProfil>();
        }

        [Key]
        public short ProfilId { get; set; }
        [Required]
        [StringLength(32)]
        public string ProfilName { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string ProfilDescription { get; set; }

        [InverseProperty(nameof(Answer.Profil))]
        public virtual ICollection<Answer> Answers { get; set; }
        [InverseProperty(nameof(UserProfil.Profil))]
        public virtual ICollection<UserProfil> UserProfils { get; set; }
    }
}
