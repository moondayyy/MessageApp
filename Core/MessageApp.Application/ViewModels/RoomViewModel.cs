using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.Application.ViewModels
{
    public class RoomViewModel
    {
        public string Timestamp { get; set; }
        public string RoomName { get; set; }
        public string Avatar { get; set; }
        public List<Guid> UserIds { get; set; }
        public List<UserViewModel> Users { get; set; }
        public Guid AdminId { get; set; }
        public UserViewModel Admin { get; set; }

    }
}
