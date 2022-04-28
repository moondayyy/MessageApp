using MessageApp.Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.Domain.Entities.Abstraction
{
    public interface IRoom
    {
        string RoomName { get; set; }
        string Avatar { get; set; }
        User Admin { get; set; }
        ICollection<MessagePrivate> Messages { get; set; }
    }
}
