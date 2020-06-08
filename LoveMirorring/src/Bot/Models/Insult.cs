using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.Models
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
