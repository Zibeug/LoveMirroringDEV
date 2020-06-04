using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class AnswerRequest
    {
        [Key]
        public short AnswerId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime AnswerDate { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string AnswerText { get; set; }
        [Required]
        [StringLength(450)]
        public string Id { get; set; }
        public short RequestId { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(AspNetUser.AnswerRequests))]
        public virtual AspNetUser IdNavigation { get; set; }
        [ForeignKey(nameof(RequestId))]
        [InverseProperty(nameof(ContactRequest.AnswerRequests))]
        public virtual ContactRequest Request { get; set; }
    }
}
