using Forum_App.Data;
using Forum_App.Models;
using Forum_App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum_App.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PostController> _logger;
        private IMemoryCache memoryCache;

        public PostController(ApplicationDbContext db, ILogger<PostController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _db = db;
            this.memoryCache = memoryCache;
        }

        // GET: PostController
        public IActionResult Index(string query)
        {
            IEnumerable<Thread> objList;

            if (!memoryCache.TryGetValue("ThreadCache", out objList))
            {
                objList = _db.Thread.Where(x => x.User_ID != null);
                var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(5));
                memoryCache.Set("ThreadCache", objList.ToList(), cacheOptions);
            }

            if (!string.IsNullOrEmpty(query))
            {
                _logger.LogInformation("Showing all threads");
                objList = objList.Where(s => s.Title.Contains(query));
            }

            foreach (Thread t in objList)
            {
                ViewData[t.Post_ID.ToString()] = _db.Comment.Where(c => c.Thread_ID == t.Post_ID).Count();
            }


            return View(objList);
        }

        // GET: PostController/Details/5
        [Route("thread/{id?}/{page?}")]
        public IActionResult Details(int id, int page)
        {
            if (page == 0)
                page = 1;
            var cm = _db.Comment.Where(c => c.Thread_ID.Equals(id)).OrderBy(c => c.CreateDate);
            ThreadCommentsViewModel model = new()
            {
                Thread = _db.Thread.Find(id),
                Comments = cm.Skip((page - 1) * 10).Take(10),
                
            };
            if (cm.Any() && !model.Comments.Any())
                return NotFound();
            ViewBag.pageCount = (cm.Count() - 1) / 10 + 1;
            ViewBag.page = page;
            ViewBag.max = (cm.Count() != 0 && cm.Count() % 10 == 0) ? true : false;
            _logger.LogInformation($"Showing details of thread {id}");
            return View(model);
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("thread/{id?}/{page?}")]
        public async Task<IActionResult> Details(int id, int page, [Bind(Prefix = "Comment")] Comment newComment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation($"New Comment on thread {id}");
                    newComment.Thread_ID = id;
                    newComment.User_ID = User.Identity.Name;                    
                    newComment.CreateDate = DateTime.UtcNow;
                    newComment.ModifyDate = newComment.CreateDate;                    
                    await _db.Post.AddAsync(newComment);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id, page });
                }
                return RedirectToAction(nameof(Details), new { id, page });
            }
            catch(Exception e)
            {
                _logger.LogError("Detailing thread went wrong " + e);
                return Details(id, page);
            }
        }

        [HttpPost]
        public JsonResult GetComments(int id, int page)
        {
            if (page == 0)
                page = 1;
            List<Comment> cm = _db.Comment.Where(c => c.Thread_ID.Equals(id)).OrderBy(c => c.CreateDate).Skip((page - 1) * 10).Take(10).ToList();

            _logger.LogInformation($"Showing details of thread {id}");

            return Json(cm);
        }

        [Authorize]
        // GET: PostController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Thread newItem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    newItem.User_ID = User.Identity.Name;
                    newItem.CreateDate = DateTime.UtcNow;
                    newItem.ModifyDate = newItem.CreateDate;
                    await _db.Post.AddAsync(newItem);
                    await _db.SaveChangesAsync();
                    _logger.LogInformation("Thread created");
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                _logger.LogError("Creating thread went wrong " + e);
                return View();
            }
        }

        [Authorize]
        // GET: PostController/Edit/5
        public IActionResult Edit(int id)
        {
            var obj = _db.Thread.Find(id);
            if(obj.User_ID == User.Identity.Name)
                return View(obj);
            return RedirectToAction(nameof(Index));
        }

        // POST: PostController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Thread obj)
        {
            Thread t = _db.Thread.AsNoTracking().Where(t => t.Post_ID == obj.Post_ID).FirstOrDefault();
            try
            {
                if (ModelState.IsValid)
                {
                    if (obj.Contents != t.Contents || obj.Title != t.Title)
                    {
                        obj.ModifyDate = DateTime.UtcNow;
                    }
                    else
                        obj.ModifyDate = t.ModifyDate;
                    _db.Post.Update(obj);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(obj);
            }
            catch (Exception e)
            {
                _logger.LogError("Editing thread went wrong " + e);
                return View();
            }
        }

        [Authorize]
        // GET: PostController/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _db.Thread.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            if (obj.User_ID == User.Identity.Name)
                return View(obj);
            return RedirectToAction(nameof(Index));
        }

        // POST: PostController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(Thread obj)
        {
            try
            {
                var orgThread = _db.Thread.FirstOrDefault(t => t.Post_ID == obj.Post_ID);
                orgThread.User_ID = null;
                await _db.SaveChangesAsync();
                _logger.LogInformation("Thread deleted successfully");
                return RedirectToAction(nameof(Index));

            }
            catch(Exception e)
            {
                _logger.LogError("Delete thread went wrong " + e);
                return NotFound();
            }
        }
    }
}
