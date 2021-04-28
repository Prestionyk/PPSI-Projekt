using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum_App.Models
{
    public class Post
    {
        [Key]
        public int Post_ID { get; set; }
        [Required]
        public string Contents { get; set; }
        public string User_ID { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}