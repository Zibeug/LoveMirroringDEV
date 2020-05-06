using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.ViewModels
{
    public class UserChoiceViewModel
    {
        public string UserName { get; set; }
        public int Age { get; set; }
        public short SexeId { get; set; }

        public short ReligionId { get; set; }
        public short HairSizeId { get; set; }
        public short HairColorId { get; set; }
        public short SexualityId { get; set; }
        public short CorpulenceId { get; set; }
        public short StyleId { get; set; }
    }
}
