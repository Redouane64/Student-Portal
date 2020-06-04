using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using StudentPortal.WebSite.Data;
using StudentPortal.WebSite.Models;
using StudentPortal.WebSite.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StudentPortal.WebSite.Controllers
{
    [Route("forums/{forumId}/topics")]
    public class TopicsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TopicsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Topics
        [HttpGet("")]
        public async Task<IActionResult> Index(Int32 forumId)
        {
            var forum = await _context.Forums.SingleOrDefaultAsync(f => f.Id == forumId);

            if(forum is null)
            {
                return NotFound();
            }

            // TODO: fix this
            var topics = _context.ForumTopics
                .AsNoTracking()
                .Where(t => t.ForumId == forumId)
                .Include(f => f.Creator)
                .Include(f => f.Forum)
                .Include(f => f.Messages)
                .ThenInclude(m => m.Creator)
                .Select(t => new ForumTopicItemViewModel { 
                    Id = t.Id,
                    Name = t.Name,
                    Created = t.Created,
                    Creator = t.Creator.Email,
                    Replies = t.Messages.Count(),
                    /*
                    LastReplyDate = t.Messages.OrderByDescending(m => m.Created).Select(m => m.Created).FirstOrDefault(),
                    LastReplyUser = t.Messages.OrderByDescending(m => m.Created).Select(m => m.Creator.Email).FirstOrDefault(),
                    */
                });

            return View(new ForumTopicsViewModel { 
                ForumId = forum.Id, 
                ForumName = forum.Name, 
                ForumDescription = forum.Description, 
                Topics = await topics.ToListAsync() 
            });
        }

        // GET: Topics/Create
        [HttpGet("create")]
        [Authorize]
        public IActionResult Create(Int32 forumId)
        {
            return View(new ForumTopic { ForumId = forumId });
        }

        // POST: Topics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Name,ForumId")] ForumTopic forumTopic)
        {
            if (ModelState.IsValid)
            {
                forumTopic.Created = DateTime.Now;
                forumTopic.CreatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _context.Add(forumTopic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { forumId = forumTopic.ForumId });
            }

            return View(forumTopic);
        }

        // GET: Topics/Edit/5
        [HttpGet("edit")]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumTopic = await _context.ForumTopics.SingleOrDefaultAsync(m => m.Id == id);
            if (forumTopic == null)
            {
                return NotFound();
            }

            return View(forumTopic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ForumId")] ForumTopic forumTopic)
        {
            if (id != forumTopic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    forumTopic.Created = DateTime.Now;
                    forumTopic.CreatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    _context.Update(forumTopic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForumTopicExists(forumTopic.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { forumId = forumTopic.ForumId });
            }

            return View(forumTopic);
        }

        // GET: Topics/Delete/5
        [HttpGet("delete")]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumTopic = await _context.ForumTopics
                .Include(f => f.Creator)
                .Include(f => f.Forum)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumTopic == null)
            {
                return NotFound();
            }

            return View(forumTopic);
        }

        // POST: Topics/Delete/5
        [HttpPost("delete"), ActionName("delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var forumTopic = await _context.ForumTopics.SingleOrDefaultAsync(m => m.Id == id);
            _context.ForumTopics.Remove(forumTopic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { forumId = forumTopic.ForumId });
        }

        private bool ForumTopicExists(int id)
        {
            return _context.ForumTopics.Any(e => e.Id == id);
        }
    }
}
