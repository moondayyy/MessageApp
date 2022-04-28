using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageApp.Application.ViewModels
{
    public class UploadViewModel
    {
        public Guid RoomId { get; set; }

        public IFormFile File { get; set; }
    }
}
