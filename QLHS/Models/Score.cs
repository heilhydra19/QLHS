using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QLHS.Models
{
    public class Score
    { 
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public double? MouthTest { get; set; }
        public double? Test15m { get; set; }
        public double? Test60m { get; set; }
        public double? OralTest { get; set; }
        public double? FinalTest { get; set; }

        public virtual Student StudentNavigation { get; set; }
        public virtual Subject SubjectNavigation { get; set; }
    }
}
