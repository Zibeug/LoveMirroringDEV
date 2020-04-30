using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class UserProfil
    {
        [Key]
        public short ProfilId { get; set; }
        [Key]
        public string Id { get; set; }
        public short? UserProfilValue { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(AspNetUser.UserProfils))]
        public virtual AspNetUser IdNavigation { get; set; }
        [ForeignKey(nameof(ProfilId))]
        [InverseProperty("UserProfils")]
        public virtual Profil Profil { get; set; }
    }
}
