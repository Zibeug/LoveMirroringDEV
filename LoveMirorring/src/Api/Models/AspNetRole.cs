using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class AspNetRole : IdentityRole
    {
        public AspNetRole()
        {
            AspNetRoleClaims = new HashSet<AspNetRoleClaim>();
            AspNetUserRoles = new HashSet<AspNetUserRole>();
        }

        [Key]
        public override string Id { get; set; }
        public override string ConcurrencyStamp { get; set; }
        [StringLength(256)]
        public override string Name { get; set; }
        [StringLength(256)]
        public override string NormalizedName { get; set; }

        [InverseProperty(nameof(AspNetRoleClaim.Role))]
        public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        [InverseProperty(nameof(AspNetUserRole.User))]
        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
    }
}
