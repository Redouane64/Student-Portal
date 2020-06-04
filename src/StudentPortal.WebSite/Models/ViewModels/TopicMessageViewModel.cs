using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentPortal.WebSite.Models.ViewModels
{
    public class TopicMessageViewModel
    {
        public int Id { get; set; }

        public string Topic { get; set; }

        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
        public string Creator { get; set; }

        public string Text { get; set; }

        public IEnumerable<MessageAttachment> Attachments { get; set; }
    }

    public class MessageAttachment
    {
        public int Id { get; set; }

        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
}
