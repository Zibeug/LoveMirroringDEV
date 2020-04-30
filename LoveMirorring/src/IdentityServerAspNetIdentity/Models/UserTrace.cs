using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class UserTrace
    {
        [Key]
        [Column("LOGID")]
        public short Logid { get; set; }
        [Column("ID")]
        [StringLength(450)]
        public string Id { get; set; }
        [Column("LOGDATE", TypeName = "datetime")]
        public DateTime Logdate { get; set; }
        [Required]
        [Column("PAGEVISITED")]
        [StringLength(128)]
        public string Pagevisited { get; set; }
        [Required]
        [Column("IPADRESS")]
        [StringLength(128)]
        public string Ipadress { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(AspNetUser.UserTraces))]
        public virtual AspNetUser IdNavigation { get; set; }
    }
}
