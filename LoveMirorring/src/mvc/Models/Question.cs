using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        [Key]
        public short QuestionId { get; set; }
        [Required]
        [StringLength(128)]
        public string QuestionText { get; set; }

        [InverseProperty(nameof(Answer.Question))]
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
