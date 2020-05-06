using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class AspNetUserToken : IdentityUserToken<string>
    {
        [Key]
        public override string UserId { get; set; }
        [Key]
        public override string LoginProvider { get; set; }
        [Key]
        public override string Name { get; set; }
        public override string Value { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AspNetUser.AspNetUserTokens))]
        public virtual AspNetUser User { get; set; }
    }
}
