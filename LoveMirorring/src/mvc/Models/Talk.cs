using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class Talk
    {
        public Talk()
        {
            Messages = new HashSet<Message>();
        }

        [Key]
        public short TalkId { get; set; }
        [Required]
        [StringLength(450)]
        public string Id { get; set; }
        [Required]
        [StringLength(450)]
        public string IdUser2Talk { get; set; }
        [Required]
        [StringLength(64)]
        public string TalkName { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(AspNetUser.TalkIdNavigations))]
        public virtual AspNetUser IdNavigation { get; set; }
        [ForeignKey(nameof(IdUser2Talk))]
        [InverseProperty(nameof(AspNetUser.TalkIdUser2TalkNavigation))]
        public virtual AspNetUser IdUser2TalkNavigation { get; set; }
        [InverseProperty(nameof(Message.Talk))]
        public virtual ICollection<Message> Messages { get; set; }
    }
}
