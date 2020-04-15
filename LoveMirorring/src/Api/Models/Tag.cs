using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public partial class Tag
    {
        public Tag()
        {
            PictureTags = new HashSet<PictureTag>();
        }

        [Key]
        public short TagId { get; set; }
        [Required]
        [StringLength(32)]
        public string TagName { get; set; }

        [InverseProperty(nameof(PictureTag.Tag))]
        public virtual ICollection<PictureTag> PictureTags { get; set; }
    }
}
