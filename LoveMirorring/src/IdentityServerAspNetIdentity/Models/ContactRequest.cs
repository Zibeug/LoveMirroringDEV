using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class ContactRequest
    {
        public ContactRequest()
        {
            AnswerRequests = new HashSet<AnswerRequest>();
        }

        [Key]
        public short RequestId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime RequestDate { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string RequestText { get; set; }
        [Required]
        [StringLength(450)]
        public string Id { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(AspNetUser.ContactRequests))]
        public virtual AspNetUser IdNavigation { get; set; }
        [InverseProperty(nameof(AnswerRequest.Request))]
        public virtual ICollection<AnswerRequest> AnswerRequests { get; set; }
    }
}
