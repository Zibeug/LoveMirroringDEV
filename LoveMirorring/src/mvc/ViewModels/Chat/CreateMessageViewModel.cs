using mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.ViewModels.Chat
{
    public class CreateMessageViewModel
    {
        public string UserId { get; set; }
        public string UserLikedId { get; set; }
        public Talk Talk { get; set; }
        public Message NewMessage { get; set; }
        public IEnumerable<GetMessagesViewModel> DisplayMessages { get; set; }
    }
}
