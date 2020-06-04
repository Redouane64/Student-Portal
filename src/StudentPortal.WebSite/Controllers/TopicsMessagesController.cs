using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using StudentPortal.WebSite.Data;
using StudentPortal.WebSite.Models;
using StudentPortal.WebSite.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace StudentPortal.WebSite.Controllers
{
    [Route("forums/topics/{topicId}/replies")]
    public class TopicsMessagesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IAuthorizationService authorizationService;
        private readonly ILogger<TopicsMessagesController> logger;
        private readonly IHostingEnvironment hostingEnvironment;

        public TopicsMessagesController(
            ApplicationDbContext context, 
            IAuthorizationService authorizationService,
            ILogger<TopicsMessagesController> logger, 
            IHostingEnvironment environment)
        {
            this.context = context;
            this.authorizationService = authorizationService;
            this.logger = logger;
            this.hostingEnvironment = environment;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(Int32? topicId)
        {
            var topic = await context.ForumTopics
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.Id == topicId);

            if(topic is null)
            {
                return NotFound();
            }

            var messages = await context.ForumMessages
                .AsNoTracking()
                .Include(m => m.ForumTopic)
                .Include(m => m.Attachments)
                .Include(m => m.Creator)
                .Where(m => m.ForumTopicId == topicId)
                .Select(m => new TopicMessageViewModel
                {
                    Id = m.Id,
                    Text = m.Text,
                    Created = m.Created,
                    LastModified = m.Modified,
                    Creator = m.Creator.Email,
                    Attachments = m.Attachments.Select(
                        a => new MessageAttachment 
                        {
                            Id = a.Id,
                            FileName = a.FileName, 
                            FilePath = a.FilePath 
                        }
                    )
                }).ToListAsync();

            ViewData["Topic"] = topic;
            return View(messages);
        }

        [HttpGet("create")]
        [Authorize]
        public async Task<IActionResult> Create(Int32 topicId)
        {
            var topic = await context.ForumTopics
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.Id == topicId);

            if (topic is null)
            {
                return NotFound();
            }

            ViewData["Topic"] = topic;

            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Int32 topicId,
                                                ForumMessage message,
                                                [FromForm]IFormFileCollection attachments)
        {
            if(topicId != message.ForumTopicId)
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return View();
            }

            message.Created = message.Modified = DateTime.Now;
            message.CreatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var files = new List<ForumMessageAttachment>();
            foreach (var attachment in attachments)
            {
                files.Add(new ForumMessageAttachment
                {
                    Created = DateTime.Now,
                    FileName = attachment.FileName,
                    FilePath = await CreateFile(attachment)
                });
            }

            message.Attachments = files;

            try
            {
                await context.AddAsync(message);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                files.ForEach(a => DeleteFile(a.FileName));
                throw;
            }

            return RedirectToActionPermanent("Index", new { topicId });
        }

        async Task<string> CreateFile(IFormFile formFile)
        {
            string path = Path.Combine(this.hostingEnvironment.WebRootPath, "attachments", formFile.FileName);

            logger.LogInformation("Writing file to {0}", path);

            using (var outFile = System.IO.File.Create(path))
            {
                await formFile.CopyToAsync(outFile);
                return path;
            }
        }

        void DeleteFile(string fileName)
        {
            string path = Path.Combine(this.hostingEnvironment.WebRootPath, "attachments", fileName);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

        }

        [HttpGet("{id:Int}/edit")]
        [Authorize]
        public async Task<IActionResult> Edit(Int32 id)
        {
            var message = await context.ForumMessages
                .AsNoTracking()
                .Include(m => m.ForumTopic)
                .SingleOrDefaultAsync(m => m.Id == id);
            
            if(message is null)
            {
                return NotFound();
            }

            var authResult = await authorizationService.AuthorizeAsync(User, message.Id, "MessageAuthor");
            if(!authResult.Succeeded)
            {
                return Unauthorized();
            }

            ViewData["Topic"] = message.ForumTopic;

            return View(message);
        }

        [HttpPost("{id:Int}/edit")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(
            Int32 topicId,
            Int32 Id,
            ForumMessage message,
            [FromForm]FormFileCollection attachments)
        {

            if (topicId != message.ForumTopicId)
            {
                return NotFound();
            }

            if (Id != message.Id)
            {
                return NotFound();
            }

            var authResult = await authorizationService.AuthorizeAsync(User, message.Id, "MessageAuthor");
            if (!authResult.Succeeded)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(message);
            }

            message.Modified = DateTime.Now;
            message.CreatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var files = new List<ForumMessageAttachment>();
            foreach (var attachment in attachments)
            {
                files.Add(new ForumMessageAttachment
                {
                    Created = DateTime.Now,
                    FileName = attachment.FileName,
                    FilePath = await CreateFile(attachment)
                });
            }

            message.Attachments = files;

            try
            {
                var entity = context.Attach(message);
                entity.Property(e => e.CreatorId).IsModified = true;
                entity.Property(e => e.Text).IsModified = true;
                entity.Property(e => e.Modified).IsModified = true;
                entity.Collection(e => e.Attachments).IsModified = true;

                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                files.ForEach(a => DeleteFile(a.FileName));
                throw;
            }

            return RedirectToActionPermanent("Index", new { topicId });
        }

        [HttpGet("{id:Int}/attach")]
        [Authorize]
        public async Task<IActionResult> Attach(Int32 id)
        {
            var message = await context.ForumMessages
                .Include(m => m.ForumTopic)
                .SingleOrDefaultAsync(m => m.Id == id);

            if(message is null)
            {
                return NotFound();
            }

            var authResult = await authorizationService.AuthorizeAsync(User, message.Id, "MessageAuthor");
            if (!authResult.Succeeded)
            {
                return Unauthorized();
            }

            return View(message);
        }

        [HttpPost("{messageId:Int}/attach")]
        [AutoValidateAntiforgeryToken]
        [Authorize]
        public async Task<IActionResult> Attach(Int32 topicId, int messageId, [FromForm]FormFileCollection attachments)
        {

            var message = await context.ForumMessages
                .Include(m => m.ForumTopic)
                .SingleOrDefaultAsync(m => m.Id == messageId);

            if (message is null)
            {
                return NotFound();
            }

            var authResult = await authorizationService.AuthorizeAsync(User, message.Id, "MessageAuthor");
            if (!authResult.Succeeded)
            {
                return Unauthorized();
            }

            if (!attachments.Any())
            {
                ModelState.AddModelError("attachments", "Attachments cannot be empty.");
                return View(message);
            }

            var files = new List<ForumMessageAttachment>();
            foreach (var attachment in attachments)
            {
                files.Add(new ForumMessageAttachment
                {
                    Created = DateTime.Now,
                    FileName = attachment.FileName,
                    FilePath = await CreateFile(attachment)
                });
            }

            try
            {
                message.Attachments = files;
                context.Entry(message).Collection(e => e.Attachments).IsModified = true;

                await context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToActionPermanent("Index", new { topicId });
        }

        [HttpGet("{messageId:int}/detach")]
        [Authorize]
        public async Task<IActionResult> Detach(Int32 topicId, Int32 messageId, Int32 attachmentId)
        {

            var authResult = await authorizationService.AuthorizeAsync(User, messageId, "MessageAuthor");
            if (!authResult.Succeeded)
            {
                return Unauthorized();
            }

            var attachment = await context.ForumMessageAttachments.SingleOrDefaultAsync(a => a.ForumMessageId == messageId && a.Id == attachmentId);

            if(attachment is null)
            {
                return NotFound();
            }

            try
            {
                context.Remove(attachment);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToActionPermanent("Index", new { topicId });
        }

        [HttpGet("{id:int}/delete")]
        [Authorize]
        public async Task<IActionResult> Delete(Int32 id)
        {
            var message = await context.ForumMessages
                .Include(m => m.ForumTopic)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (message is null)
            {
                return NotFound();
            }

            var authResult = await authorizationService.AuthorizeAsync(User, message.Id, "MessageAuthor");
            if (!authResult.Succeeded)
            {
                return Unauthorized();
            }

            ViewData["TopicId"] = message.ForumTopicId;
            ViewData["MessageId"] = message.Id;

            return View();
        }

        [HttpPost("{id:int}/delete")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirm(Int32 topicId, Int32 id)
        {
            var message = await context.ForumMessages
                    .Include(m => m.ForumTopic)
                    .SingleOrDefaultAsync(m => m.Id == id);

            if (message is null)
            {
                return NotFound();
            }

            var authResult = await authorizationService.AuthorizeAsync(User, message.Id, "MessageAuthor");
            if (!authResult.Succeeded)
            {
                return Unauthorized();
            }

            try
            {
                context.Remove(message);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToActionPermanent("Index", new { topicId });
        }
    }
}