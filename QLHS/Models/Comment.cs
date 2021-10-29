using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QLHS.Models
{
    public class Comment
    {
        public int StudentId { get; set; }
        public int PostId { get; set; }
        public string CommentContent { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual NewFeed PostNavigation { get; set; } 
        public virtual Student StudentNavigation { get; set; }
    }
}
