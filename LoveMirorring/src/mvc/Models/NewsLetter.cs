using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class Newsletter
    {
        public Newsletter()
        {
            UserNewsletters = new HashSet<UserNewsletter>();
        }

        [Key]
        public short NewsletterId { get; set; }
        [Required]
        [StringLength(32)]
        public string NewsletterName { get; set; }
        public bool NewsletterStatus { get; set; }

        [InverseProperty(nameof(UserNewsletter.Newsletter))]
        public virtual ICollection<UserNewsletter> UserNewsletters { get; set; }
    }
}
