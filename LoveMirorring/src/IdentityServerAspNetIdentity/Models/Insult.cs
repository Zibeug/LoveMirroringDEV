using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class Insult
    {
        [Key]
        [Column("InsultID")]
        public int InsultId { get; set; }
        [StringLength(450)]
        public string InsultName { get; set; }
    }
}
