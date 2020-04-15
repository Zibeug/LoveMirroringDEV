using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    [Table("PictureTag")]
    public partial class PictureTag
    {
        [Key]
        public short PictureId { get; set; }
        [Key]
        public short TagId { get; set; }

        [ForeignKey(nameof(PictureId))]
        [InverseProperty("PictureTags")]
        public virtual Picture Picture { get; set; }
        [ForeignKey(nameof(TagId))]
        [InverseProperty("PictureTags")]
        public virtual Tag Tag { get; set; }
    }
}
