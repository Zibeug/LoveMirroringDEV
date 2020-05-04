using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class UserSubscription
    {
        [Key]
        [Column("SubscriptionID")]
        public int SubscriptionId { get; set; }
        [Column("UserID")]
        [StringLength(450)]
        public string UserId { get; set; }
        [Column("SubscriptionDATE", TypeName = "datetime")]
        public DateTime SubscriptionDate { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal SubscriptionAmount { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AspNetUser.UserSubscriptions))]
        public virtual AspNetUser User { get; set; }
    }
}
