using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class Tag
    {
        public Tag()
        {
            PicturesTags = new HashSet<PicturesTag>();
        }

        [Key]
        public short TagId { get; set; }
        [Required]
        [StringLength(32)]
        public string TagName { get; set; }

        [InverseProperty(nameof(PicturesTag.Tag))]
        public virtual ICollection<PicturesTag> PicturesTags { get; set; }
    }
}
