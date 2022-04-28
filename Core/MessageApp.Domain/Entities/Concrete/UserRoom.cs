using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.Domain.Entities.Concrete
{
    public class UserRoom
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public Room Room { get; set; }
        [ForeignKey("Room")]
        public Guid RoomId { get; set; }
    }
}
