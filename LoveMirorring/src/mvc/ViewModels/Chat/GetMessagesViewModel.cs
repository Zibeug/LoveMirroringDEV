using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvc.ViewModels.Chat
{
    public class GetMessagesViewModel
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}
