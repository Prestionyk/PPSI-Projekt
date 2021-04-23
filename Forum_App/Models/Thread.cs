using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum_App.Models
{
    public class Thread : Post
    {
        public string Title { get; set; }
    }
}
