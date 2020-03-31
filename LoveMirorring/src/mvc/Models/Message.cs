using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class Message
    {
        [Key]
        public short MessageId { get; set; }
        [Required]
        [StringLength(450)]
        public string Id { get; set; }
        public short TalkId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime MessageDate { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(AspNetUser.Messages))]
        public virtual AspNetUser IdNavigation { get; set; }
        [ForeignKey(nameof(TalkId))]
        [InverseProperty("Messages")]
        public virtual Talk Talk { get; set; }
    }
}
