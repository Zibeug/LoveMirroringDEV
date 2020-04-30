using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class UserStyle
    {
        [Key]
        public string Id { get; set; }
        [Key]
        public short StyleId { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(AspNetUser.UserStyles))]
        public virtual AspNetUser IdNavigation { get; set; }
        [ForeignKey(nameof(StyleId))]
        [InverseProperty("UserStyles")]
        public virtual Style Style { get; set; }
    }
}
