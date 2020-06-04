using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentPortal.WebSite.Data;
using StudentPortal.WebSite.Models;
using Microsoft.AspNetCore.Authorization;

namespace StudentPortal.WebSite.Controllers
{
    [Route("forums/categories")]
    [Authorize(Roles = ApplicationRoles.Administrators)]
    public class ForumCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ForumCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ForumCategories/Create
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ForumCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] ForumCategory forumCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(forumCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Forums");
            }
            return View(forumCategory);
        }

        // GET: ForumCategories/Edit/5
        [HttpGet("edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumCategory = await _context.ForumCategories.SingleOrDefaultAsync(m => m.Id == id);
            if (forumCategory == null)
            {
                return NotFound();
            }
            return View(forumCategory);
        }

        // POST: ForumCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Name")] ForumCategory forumCategory)
        {
            if (id != forumCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(forumCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForumCategoryExists(forumCategory.Name))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Forums");
            }
            return View(forumCategory);
        }

        // GET: ForumCategories/Delete/5
        [HttpGet("delete")]
        public async Task<IActionResult> Delete(Int32 id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumCategory = await _context.ForumCategories
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumCategory == null)
            {
                return NotFound();
            }

            return View(forumCategory);
        }

        // POST: ForumCategories/Delete/5
        [HttpPost("delete"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var forumCategory = await _context.ForumCategories.SingleOrDefaultAsync(m => m.Name == id);
            _context.ForumCategories.Remove(forumCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Forums");
        }

        private bool ForumCategoryExists(string id)
        {
            return _context.ForumCategories.Any(e => e.Name == id);
        }
    }
}
