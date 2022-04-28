using MessageApp.Domain.Entities.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.Domain.Entities.Concrete
{
    public class MessagePrivate : Base
    {
        private string _Content;
        private User _FromUser;
        private User _ToUser;
        private Guid _FromUserId;
        private Guid _ToUserId;

        public string Content { get => _Content; set => _Content = value; }

        [ForeignKey("FromUser")]
        public Guid FromUserId { get => _FromUserId; set => _FromUserId = value; }

        [ForeignKey("ToUser")]
        public Guid ToUserId { get => _ToUserId; set => _ToUserId = value; }

        //public virtual AppUser ToUser { get => _ToUser; set => _ToUser = value; }
        public User FromUser { get => _FromUser; set => _FromUser = value; }
    }
}
