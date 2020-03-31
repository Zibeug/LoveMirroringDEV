using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class Sex
    {
        public Sex()
        {
            AspNetUsers = new HashSet<AspNetUser>();
        }

        [Key]
        public short SexeId { get; set; }
        [Required]
        [StringLength(32)]
        public string SexeName { get; set; }

        [InverseProperty(nameof(AspNetUser.Sexe))]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
