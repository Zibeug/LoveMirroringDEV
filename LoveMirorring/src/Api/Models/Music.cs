using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class Music
    {
        public Music()
        {
            PreferenceMusics = new HashSet<PreferenceMusic>();
            UserMusics = new HashSet<UserMusic>();
        }

        [Key]
        public short MusicId { get; set; }
        [Required]
        [StringLength(32)]
        public string MusicName { get; set; }
        [Required]
        [StringLength(64)]
        public string ArtistName { get; set; }

        [InverseProperty(nameof(PreferenceMusic.Music))]
        public virtual ICollection<PreferenceMusic> PreferenceMusics { get; set; }
        [InverseProperty(nameof(UserMusic.Music))]
        public virtual ICollection<UserMusic> UserMusics { get; set; }
    }
}
