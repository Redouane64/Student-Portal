using System;
using System.Linq;
using System.Threading.Tasks;

using StudentPortal.WebSite.Data;
using StudentPortal.WebSite.Models;
using StudentPortal.WebSite.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace StudentPortal.WebSite.Controllers
{
    [Route("forums")]
    public class ForumsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ForumsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Forums
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var allForums = await _context.Forums
                .Include(f => f.ForumCategory)
                .Include(f => f.Topics)
                .Select(f => new {
                    Category = f.ForumCategory, 
                    Topics = f.Topics.Count(), 
                    Forum = new { 
                            f.Name,
                            f.Id,
                            f.Description
                        } 
                    }
                ).ToListAsync();

            var groupedForums = allForums.GroupBy(
                f => f.Category, 
                f => new ForumItemViewModel { 
                    Description = f.Forum.Description,
                    Topics = f.Topics,
                    Id = f.Forum.Id,
                    Name = f.Forum.Name
                })
                .Select(c => new ForumCategoryViewModel(c.Key.Name, c.Key.Id, c.ToArray()));

            return View(groupedForums);
        }

        // GET: Forums/Create
        [HttpGet("create")]
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public IActionResult Create(Int32? categoryId)
        {
            ViewData["ForumCategoryId"] = new SelectList(_context.ForumCategories, "Id", "Name");
            return View(new Forum { ForumCategoryId = categoryId.Value });
        }

        // POST: Forums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ForumCategoryId")] Forum forum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(forum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ForumCategoryId"] = new SelectList(_context.ForumCategories, "Id", "Name", forum.ForumCategoryId);
            return View(forum);
        }

        // GET: Forums/Edit/5
        [HttpGet("edit")]
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forums.SingleOrDefaultAsync(m => m.Id == id);
            if (forum == null)
            {
                return NotFound();
            }
            ViewData["ForumCategoryId"] = new SelectList(_context.ForumCategories, "Id", "Name", forum.ForumCategoryId);
            return View(forum);
        }

        // POST: Forums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ForumCategoryId")] Forum forum)
        {
            if (id != forum.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(forum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForumExists(forum.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ForumCategoryId"] = new SelectList(_context.ForumCategories, "Name", "Name", forum.ForumCategoryId);
            return View(forum);
        }

        // GET: Forums/Delete/5
        [HttpGet("delete")]
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forums
                .Include(f => f.ForumCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forum == null)
            {
                return NotFound();
            }

            return View(forum);
        }

        // POST: Forums/Delete/5
        [HttpPost("delete"), ActionName("delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var forum = await _context.Forums.SingleOrDefaultAsync(m => m.Id == id);
            _context.Forums.Remove(forum);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ForumExists(int id)
        {
            return _context.Forums.Any(e => e.Id == id);
        }
    }
}
