using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class AspNetUserLogin : IdentityUserLogin<string>
    {
        [Key]
        [StringLength(450)]
        public override string LoginProvider { get; set; }
        [Key]
        [StringLength(450)]
        public override string ProviderKey { get; set; }
        [Required]
        [StringLength(450)]
        public override string UserId { get; set; }
        [Column(TypeName = "text")]
        public override string ProviderDisplayName { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AspNetUser.AspNetUserLogins))]
        public virtual AspNetUser User { get; set; }
    }
}
