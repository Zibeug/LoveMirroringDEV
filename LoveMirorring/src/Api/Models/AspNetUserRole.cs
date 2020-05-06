using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class AspNetUserRole : IdentityUserRole<string>
    {
        [Key]
        public override string UserId { get; set; }
        [Key]
        public override string RoleId { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AspNetRole.AspNetUserRoles))]
        public virtual AspNetRole User { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AspNetUser.AspNetUserRoles))]
        public virtual AspNetUser UserNavigation { get; set; }
    }
}
