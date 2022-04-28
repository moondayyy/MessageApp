using MessageApp.Domain.Entities.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.Domain.Entities.Concrete
{
    public class MessageGroup : Base
    {
        private string _Content;
        private User _FromUser;
        private User _ToUser;
        private Room? _ToRoom;
        private Guid _ToRoomId;
        private Guid _FromUserId;
        private Guid _ToUserId;

        public string Content { get => _Content; set => _Content = value; }

        public User FromUser { get => _FromUser; set => _FromUser = value; }
        [ForeignKey("FromUser")]
        public Guid FromUserId { get => _FromUserId; set => _FromUserId = value; }

        public Room? ToRoom { get => _ToRoom; set => _ToRoom = value; }
        [ForeignKey("ToRoom")]
        public Guid ToRoomId { get => _ToRoomId; set => _ToRoomId = value; }
    }
}
