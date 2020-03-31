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
            UsersExternalServices = new HashSet<UsersExternalService>();
        }

        [Key]
        public short ExternalServiceId { get; set; }
        [Required]
        [StringLength(32)]
        public string ExternalServiceName { get; set; }
        public bool ExternalServiceStatus { get; set; }

        [InverseProperty(nameof(UsersExternalService.ExternalService))]
        public virtual ICollection<UsersExternalService> UsersExternalServices { get; set; }
    }
}
