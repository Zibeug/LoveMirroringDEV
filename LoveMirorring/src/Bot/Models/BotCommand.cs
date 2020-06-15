using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.Models
{
    public partial class BotCommand
    {
        [Key]
        public short Id { get; set; }
        [Required]
        [Column("name")]
        public string Name { get; set; }
        [Required]
        [Column("slug")]
        [StringLength(50)]
        public string Slug { get; set; }
        [Column("answer")]
        public string Answer { get; set; }
    }
}
