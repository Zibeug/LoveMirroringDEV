using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class PicturesTag
    {
        [Key]
        public short PictureId { get; set; }
        [Key]
        public short TagId { get; set; }

        [ForeignKey(nameof(PictureId))]
        [InverseProperty("PicturesTags")]
        public virtual Picture Picture { get; set; }
        [ForeignKey(nameof(TagId))]
        [InverseProperty("PicturesTags")]
        public virtual Tag Tag { get; set; }
    }
}
