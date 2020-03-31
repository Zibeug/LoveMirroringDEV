using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class UsersPreference
    {
        [Key]
        public short PreferenceId { get; set; }
        [Key]
        public string Id { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(AspNetUser.UsersPreferences))]
        public virtual AspNetUser IdNavigation { get; set; }
        [ForeignKey(nameof(PreferenceId))]
        [InverseProperty("UsersPreferences")]
        public virtual Preference Preference { get; set; }
    }
}
