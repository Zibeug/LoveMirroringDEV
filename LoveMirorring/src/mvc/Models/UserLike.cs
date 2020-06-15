using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class UserLike
    {
        [Key]
        public string Id { get; set; }
        [Key]
        [Column("ID_1")]
        public string Id1 { get; set; }
        public bool? Ignored { get; set; }

        [ForeignKey(nameof(Id1))]
        [InverseProperty(nameof(AspNetUser.UserLikeId1Navigation))]
        public virtual AspNetUser Id1Navigation { get; set; }
        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(AspNetUser.UserLikeIdNavigations))]
        public virtual AspNetUser IdNavigation { get; set; }
    }
}
