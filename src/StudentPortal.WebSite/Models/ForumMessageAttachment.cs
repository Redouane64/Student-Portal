using System;
using System.ComponentModel.DataAnnotations;

namespace StudentPortal.WebSite.Models
{
    public class ForumMessageAttachment
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string FilePath { get; set; }

        public int ForumMessageId { get; set; }
        public ForumMessage ForumMessage { get; set; }

    }
}
