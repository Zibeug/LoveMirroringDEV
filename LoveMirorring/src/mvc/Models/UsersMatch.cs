using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    [Table("UsersMatch")]
    public partial class UsersMatch
    {
        [Key]
        [Column("ID")]
        public string Id { get; set; }
        [Key]
        [Column("ID_1")]
        public string Id1 { get; set; }

        [ForeignKey(nameof(Id1))]
        [InverseProperty(nameof(AspNetUser.UsersMatchId1Navigation))]
        public virtual AspNetUser Id1Navigation { get; set; }
        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(AspNetUser.UsersMatchIdNavigations))]
        public virtual AspNetUser IdNavigation { get; set; }
    }
}
