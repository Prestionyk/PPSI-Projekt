using Forum_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum_App.ViewModels
{
    public class ThreadViewModel
    {
        public Thread Thread { get; set; }        
        public int CommentCount { get; set; }
    }
}
