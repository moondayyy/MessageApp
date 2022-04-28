using MessageApp.Domain.Entities.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.Domain.Entities.Concrete
{
    public class Base:IBase
    {
        private Guid _Id;
        private DateTime _CreatedDate;
        private DateTime _ModifiedDate;
        private bool _IsActive;

        public Guid Id { get => _Id; set => _Id = value; }
        public DateTime CreatedDate { get => _CreatedDate; set => _CreatedDate = value; }
        public DateTime ModifiedDate { get => _ModifiedDate; set => _ModifiedDate = value; }
        public bool IsActive { get => _IsActive; set => _IsActive = value; }
    }
}
