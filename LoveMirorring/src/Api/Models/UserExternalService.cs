using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class UserExternalService
    {
        [Key]
        public short ExternalServiceId { get; set; }
        [Key]
        public string Id { get; set; }

        [ForeignKey(nameof(ExternalServiceId))]
        [InverseProperty("UserExternalServices")]
        public virtual ExternalService ExternalService { get; set; }
        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(AspNetUser.UserExternalServices))]
        public virtual AspNetUser IdNavigation { get; set; }
    }
}
