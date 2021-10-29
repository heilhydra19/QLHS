using System;
using System.Collections.Generic;

namespace QLHS.Models
{
    public class Comment
    {
        public int StudenId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }

        public virtual NewFeed Post { get; set; }
        public virtual Student Studen { get; set; }
    }
}
