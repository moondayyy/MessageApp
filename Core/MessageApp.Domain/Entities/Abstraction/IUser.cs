using MessageApp.Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.Domain.Entities.Abstraction
{
    public interface IUser
    {
        string PhoneNumber { get; set; }
        string FullName { get; set; }
        string Description { get; set; }
        string? Avatar { get; set; }
        string ConnectionId { get; set; }
        ICollection<Room> Rooms { get; set; }
        ICollection<MessagePrivate> Messages { get; set; }
    }
}
