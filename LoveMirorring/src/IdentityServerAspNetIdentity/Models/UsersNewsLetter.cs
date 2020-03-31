using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class UsersNewsLetter
    {
        [Key]
        public short NewsLetterId { get; set; }
        [Key]
        public string Id { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(AspNetUser.UsersNewsLetters))]
        public virtual AspNetUser IdNavigation { get; set; }
        [ForeignKey(nameof(NewsLetterId))]
        [InverseProperty("UsersNewsLetters")]
        public virtual NewsLetter NewsLetter { get; set; }
    }
}
