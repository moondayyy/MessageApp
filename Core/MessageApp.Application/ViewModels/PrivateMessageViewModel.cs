using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.Application.ViewModels
{
    public class PrivateMessageViewModel
    {
        public string Content { get; set; }
        public string Timestamp { get; set; }
        public string FromFullName { get; set; }
        public string FromPhoneNumber { get; set; }
        public string ReciverPhoneNumber { get; set; }
        public string Avatar { get; set; }
    }
}
