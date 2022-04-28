using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.Application.ViewModels
{
    public class UserViewModel
    {
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string? Avatar { get; set; }
        //public string ConnectionId { get; set; }
    }
}
