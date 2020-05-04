using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class Subscription
    {
        public Subscription()
        {
            AspNetUsers = new HashSet<AspNetUser>();
            UserSubscriptions = new HashSet<UserSubscription>();
        }

        [Key]
        public short SubscriptionId { get; set; }
        [Required]
        [StringLength(32)]
        public string SubscriptionName { get; set; }
        [Column(TypeName = "money")]
        public decimal SubscriptionPrice { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime SubscriptionDate { get; set; }

        [InverseProperty(nameof(AspNetUser.Subscription))]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        [InverseProperty(nameof(UserSubscription.Subscriptions))]
        public virtual ICollection<UserSubscription> UserSubscriptions { get; set; }
    }
}
