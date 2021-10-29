using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QLHS.Models
{
    public class NewFeed
    {
        public NewFeed()
        {
            Comments = new HashSet<Comment>();
        } 
        public int Id { get; set; }
        public string Img { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
