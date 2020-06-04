using System.Collections.Generic;

namespace StudentPortal.WebSite.Models
{
    public class ForumCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Forum> Forums { get; set; }
    }
}
