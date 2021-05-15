using Forum_App.Data;
using Forum_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<PostsAPIController> _logger;
        public PostsAPIController(ApplicationDbContext db, ILogger<PostsAPIController> logger)
        {
            _db = db;
            _logger = logger;
        }
        
        [Route("threads")]
        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetAllThreads()
        {
            _logger.LogInformation("Get API all threads correct");
            return _db.Thread;
        }

        [Route("threads/{ThreadID}")]
        [HttpGet]
        public ActionResult<Post> GetThread(int ThreadID)
        {
            _logger.LogInformation("Get API specified thread correct");
            return _db.Thread.FirstOrDefault(t => t.Post_ID.Equals(ThreadID));
        }

        [Route("threads/{ThreadID}/comments")]
        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetCommentsOfThread(int ThreadID)
        {
            _logger.LogInformation("Get API specified threads' comments correct");
            return _db.Comment.Where(t => t.Thread_ID.Equals(ThreadID)).ToList();
        }

        [Route("comment/{CommentID}")]
        [HttpGet]
        public ActionResult<Post> GetComment(int CommentID)
        {
            _logger.LogInformation("Get API specified comment correct");
            return _db.Comment.FirstOrDefault(t => t.Post_ID.Equals(CommentID));
        }

    }
}
