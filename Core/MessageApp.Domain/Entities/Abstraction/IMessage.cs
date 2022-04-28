using MessageApp.Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.Domain.Entities.Abstraction
{
    public interface IMessage
    {
        string Content { get; set; }
        User FromUser { get; set; }
        Room ToRoom { get; set; }

    }
}
