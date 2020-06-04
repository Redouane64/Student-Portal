using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StudentPortal.WebSite.Controllers
{
    public class MockupsController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult AllForums()
        {
            return this.View();
        }

        public IActionResult SingleForum()
        {
            return this.View();
        }

        public IActionResult SingleTopic()
        {
            return this.View();
        }
    }
}
