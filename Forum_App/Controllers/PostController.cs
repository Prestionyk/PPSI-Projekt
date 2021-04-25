using Forum_App.Data;
using Forum_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            var obj = _db.Thread.Find(id);
            return View(obj);
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
                    newItem.User_ID = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
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
        // GET: PostController/Edit/5
        public IActionResult Edit(int id)
        {
            var obj = _db.Thread.Find(id);
            return View(obj);
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
            return View(obj);
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
