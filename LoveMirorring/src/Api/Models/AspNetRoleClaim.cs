using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class AspNetRoleClaim : IdentityRoleClaim<string>
    {
        [Key]
        public override int Id { get; set; }
        [Required]
        [StringLength(450)]
        public override string RoleId { get; set; }
        public override string ClaimType { get; set; }
        public override string ClaimValue { get; set; }

        [ForeignKey(nameof(RoleId))]
        [InverseProperty(nameof(AspNetRole.AspNetRoleClaims))]
        public virtual AspNetRole Role { get; set; }
    }
}
