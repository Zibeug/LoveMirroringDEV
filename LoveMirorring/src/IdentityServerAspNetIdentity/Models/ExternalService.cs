using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class ExternalService
    {
        public ExternalService()
        {
            UserExternalServices = new HashSet<UserExternalService>();
        }

        [Key]
        public short ExternalServiceId { get; set; }
        [Required]
        [StringLength(32)]
        public string ExternalServiceName { get; set; }
        public bool ExternalServiceStatus { get; set; }

        [InverseProperty(nameof(UserExternalService.ExternalService))]
        public virtual ICollection<UserExternalService> UserExternalServices { get; set; }
    }
}
