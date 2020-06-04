using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentPortal.WebSite.Models.ViewModels
{
    public class ForumCategoryViewModel
    {
        public int CategoryId { get; set; }
        public string Category { get; set; }

        public IEnumerable<ForumItemViewModel> Forums { get; set; }

        public ForumCategoryViewModel(string category, int categoryId, IEnumerable<ForumItemViewModel> items)
        {
            this.CategoryId = categoryId;
            this.Category = category;
            this.Forums = items;
        }
    }

    public class ForumItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Topics { get; set; }
    }
}
