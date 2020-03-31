using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class UsersProfil
    {
        [Key]
        public short ProfilId { get; set; }
        [Key]
        public string Id { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(AspNetUser.UsersProfils))]
        public virtual AspNetUser IdNavigation { get; set; }
        [ForeignKey(nameof(ProfilId))]
        [InverseProperty("UsersProfils")]
        public virtual Profil Profil { get; set; }
    }
}
