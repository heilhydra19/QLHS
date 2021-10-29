using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QLHS.Models
{
    public class Student
    {
        public Student()
        {
            Comments = new HashSet<Comment>();
            Scores = new HashSet<Score>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public DateTime BirthDay { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
    }
}
