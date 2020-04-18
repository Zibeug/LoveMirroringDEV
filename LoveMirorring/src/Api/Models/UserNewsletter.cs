using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class UserNewsletter
    {
        [Key]
        public short NewsletterId { get; set; }
        [Key]
        public string Id { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(AspNetUser.UserNewsletters))]
        public virtual AspNetUser IdNavigation { get; set; }
        [ForeignKey(nameof(NewsletterId))]
        [InverseProperty("UserNewsletters")]
        public virtual Newsletter Newsletter { get; set; }
    }
}
