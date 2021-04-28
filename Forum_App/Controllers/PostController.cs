using Forum_App.Data;
using Forum_App.Models;
using Forum_App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;

namespace Forum_App.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PostController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: PostController
        public IActionResult Index(string query)
        {
            IEnumerable<Thread> objList = _db.Thread;

            if (!String.IsNullOrEmpty(query))
            {
                objList = objList.Where(s => s.Title.Contains(query));
            }

            return View(objList);
        }
        
        // GET: PostController/Details/5
        public IActionResult Details(int id)
        {
            ThreadCommentsViewModel model = new ThreadCommentsViewModel()
            {
                Thread = _db.Thread.Find(id),
                Comments = _db.Comment.Where(c => c.Thread_ID.Equals(id)),
            };

            return View(model);
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(int id, [Bind(Prefix = "Comment")] Comment newComment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    newComment.Thread_ID = id;
                    newComment.User_ID = User.Identity.Name;
                    newComment.CreateDate = DateTime.UtcNow;
                    newComment.ModifyDate = newComment.CreateDate;
                    _db.Post.Add(newComment);
                    _db.SaveChanges();
                    return RedirectToAction(nameof(Details), id);
                }
                return RedirectToAction(nameof(Details), id);
            }
            catch
            {
                return Details(id);
            }
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
        public IActionResult Create(Thread newItem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    newItem.User_ID = User.Identity.Name;
                    newItem.CreateDate = DateTime.UtcNow;
                    newItem.ModifyDate = newItem.CreateDate;
                    _db.Post.Add(newItem);
                    _db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
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
        public IActionResult Edit(Thread obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    obj.ModifyDate = DateTime.UtcNow;
                    _db.Post.Update(obj);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(obj);
            }
            catch
            {
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
        public IActionResult DeletePost(Thread obj)
        {
            try
            {

                if (obj == null)
                {
                    return NotFound("Null");
                }
                _db.Thread.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
