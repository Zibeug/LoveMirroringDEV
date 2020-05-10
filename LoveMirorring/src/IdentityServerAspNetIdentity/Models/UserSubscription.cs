using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class UserSubscription
    {
        [Key]
        [Column("UserSubscriptionsID")]
        public int UserSubscriptionsId { get; set; }
        [Column("UserID")]
        [StringLength(450)]
        public string UserId { get; set; }
        [Column("UserSubscriptionsDATE", TypeName = "datetime")]
        public DateTime UserSubscriptionsDate { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal UserSubscriptionsAmount { get; set; }
        [Column("SubscriptionsID")]
        public short SubscriptionsId { get; set; }

        [ForeignKey(nameof(SubscriptionsId))]
        [InverseProperty(nameof(Subscription.UserSubscriptions))]
        public virtual Subscription Subscriptions { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AspNetUser.UserSubscriptions))]
        public virtual AspNetUser User { get; set; }
    }
}
