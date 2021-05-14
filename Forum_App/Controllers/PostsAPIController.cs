using Forum_App.Data;
using Forum_App.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum_App.Controllers
{
    [Route("api")]
    [ApiController]
    public class PostsAPIController : Controller
    {
        private readonly ApplicationDbContext _db;
        public PostsAPIController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        [Route("threads")]
        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetAllThreads()
        {
            return _db.Thread;
        }

        [Route("threads/{ThreadID}")]
        [HttpGet]
        public ActionResult<Post> GetThread(int ThreadID)
        {
            return _db.Thread.FirstOrDefault(t => t.Post_ID.Equals(ThreadID));
        }

        [Route("threads/{ThreadID}/comments")]
        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetCommentsOfThread(int ThreadID)
        {
            return _db.Comment.Where(t => t.Thread_ID.Equals(ThreadID)).ToList();
        }

        [Route("comment/{CommentID}")]
        [HttpGet]
        public ActionResult<Post> GetComment(int CommentID)
        {
            return _db.Comment.FirstOrDefault(t => t.Post_ID.Equals(CommentID));
        }

    }
}
