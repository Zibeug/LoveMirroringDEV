using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class Picture
    {
        public Picture()
        {
            PictureTags = new HashSet<PictureTag>();
        }

        [Key]
        public short PictureId { get; set; }
        [Required]
        [StringLength(450)]
        public string Id { get; set; }
        [Required]
        [Column(TypeName = "image")]
        public byte[] PictureView { get; set; }

        [ForeignKey(nameof(Id))]
        [InverseProperty(nameof(AspNetUser.Pictures))]
        public virtual AspNetUser IdNavigation { get; set; }
        [InverseProperty(nameof(PictureTag.Picture))]
        public virtual ICollection<PictureTag> PictureTags { get; set; }
    }
}
