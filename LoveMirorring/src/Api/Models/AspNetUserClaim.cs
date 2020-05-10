using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class AspNetUserClaim : IdentityUserClaim<string>
    {
        [Key]
        public override int Id { get; set; }
        [Required]
        [StringLength(450)]
        public override string UserId { get; set; }
        public override string ClaimType { get; set; }
        public override string ClaimValue { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AspNetUser.AspNetUserClaims))]
        public virtual AspNetUser User { get; set; }
    }
}
