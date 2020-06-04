using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace StudentPortal.WebSite.Models.ViewModels
{
    public class PostAttachmentEditModel
    {
        public IFormFile File { get; set; }
    }
}
