using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class Answer
    {
        [Key]
        [Column("AnswerID")]
        public short AnswerId { get; set; }
        public short ProfilId { get; set; }
        public short QuestionId { get; set; }
        [Required]
        [StringLength(128)]
        public string AnswerText { get; set; }
        public short AnswerValue { get; set; }

        [ForeignKey(nameof(ProfilId))]
        [InverseProperty("Answers")]
        public virtual Profil Profil { get; set; }
        [ForeignKey(nameof(QuestionId))]
        [InverseProperty("Answers")]
        public virtual Question Question { get; set; }
    }
}
