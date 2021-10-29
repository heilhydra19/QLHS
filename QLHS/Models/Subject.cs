using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QLHS.Models
{
    public class Subject
    {
        public Subject()
        {
            Scores = new HashSet<Score>();
        } 
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Score> Scores { get; set; }
    }
}
