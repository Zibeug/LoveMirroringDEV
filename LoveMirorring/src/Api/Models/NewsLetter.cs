using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class NewsLetter
    {
        public NewsLetter()
        {
            UsersNewsLetters = new HashSet<UsersNewsLetter>();
        }

        [Key]
        public short NewsLetterId { get; set; }
        [Required]
        [StringLength(32)]
        public string NewsLetterName { get; set; }

        [InverseProperty(nameof(UsersNewsLetter.NewsLetter))]
        public virtual ICollection<UsersNewsLetter> UsersNewsLetters { get; set; }
    }
}
