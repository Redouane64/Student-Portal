using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPortal.WebSite.Models
{
    public class ForumMessage
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        [Required]
        public string Text { get; set; }

        [ForeignKey(nameof(ForumTopic))]
        public int ForumTopicId { get; set; }

        public ForumTopic ForumTopic { get; set; }

        public string CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }

        public ICollection<ForumMessageAttachment> Attachments { get; set; }

    }
}
