using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentPortal.WebSite.Models
{
    public class ForumTopic
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        [Required]
        public string Name { get; set; }

        public int ForumId { get; set; }
        public Forum Forum { get; set; }

        public ICollection<ForumMessage> Messages { get; set; }

        public string CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }

    }
}
