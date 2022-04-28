using MessageApp.Domain.Entities.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.Domain.Entities.Concrete
{
    public class Room : Base
    {
        private string _RoomName;
        private string _Avatar;
        private Guid _AdminId;
        private ICollection<MessageGroup> _MessagesGroup;
        private ICollection<User> _Users;

        public string RoomName { get => _RoomName; set => _RoomName = value; }
        public string Avatar { get => _Avatar; set => _Avatar = value; }
        public Guid AdminId { get => _AdminId; set => _AdminId = value; }
        public ICollection<MessageGroup> MessagesGroup { get => _MessagesGroup; set => _MessagesGroup = value; }
        public ICollection<User> Users { get => _Users; set => _Users = value; }
    }
}
