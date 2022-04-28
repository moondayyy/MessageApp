using MessageApp.Domain.Entities.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.Domain.Entities.Concrete
{
    public class User : Base
    {

        private string _PhoneNumber;
        private string _FullName;
        private string _Description;
        private string? _Avatar;
        private string _ConnectionId;
        private ICollection<Room> _Rooms;
        private ICollection<MessagePrivate> _MessagesPrivate;
        private ICollection<MessageGroup> _MessagesGroup;

        public string PhoneNumber { get => _PhoneNumber; set => _PhoneNumber = value; }
        public string FullName { get => _FullName; set => _FullName = value; }
        public string Description { get => _Description; set => _Description = value; }
        public string? Avatar { get => _Avatar; set => _Avatar = value; }
        public string ConnectionId { get => _ConnectionId; set => _ConnectionId = value; }

        public ICollection<MessagePrivate> MessagesPrivate { get => _MessagesPrivate; set => _MessagesPrivate = value; }
        public ICollection<MessageGroup> MessagesGroup { get => _MessagesGroup; set => _MessagesGroup = value; }
        public ICollection<Room> Rooms { get => _Rooms; set => _Rooms = value; }

    }
}
