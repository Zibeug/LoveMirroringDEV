using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class UserMusic
    {
        [Key]
        public string Id { get; set; }
        [Key]
        public short MusicId { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(AspNetUser.UserMusics))]
        public virtual AspNetUser IdNavigation { get; set; }
        [ForeignKey(nameof(MusicId))]
        [InverseProperty("UserMusics")]
        public virtual Music Music { get; set; }
    }
}
