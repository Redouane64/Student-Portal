using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentPortal.WebSite.Models
{
    public class Forum
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Category")]
        public int ForumCategoryId { get; set; }

        [Display(Name = "Category")]
        public ForumCategory ForumCategory { get; set; }

        public ICollection<ForumTopic> Topics { get; set; }
    }
}
