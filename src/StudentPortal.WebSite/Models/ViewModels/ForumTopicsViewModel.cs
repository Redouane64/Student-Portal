using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentPortal.WebSite.Models.ViewModels
{
    public class ForumTopicsViewModel
    {
        public int ForumId { get; set; }

        public string ForumName { get; set; }
        public string ForumDescription { get; set; }

        public IEnumerable<ForumTopicItemViewModel> Topics { get; set; }


    }

    public class ForumTopicItemViewModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public int Replies { get; set; }
        public string LastReplyUser { get; set; }
        public DateTime LastReplyDate { get; set; }
    }
}
