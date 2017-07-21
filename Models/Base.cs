using System;
using System.ComponentModel.DataAnnotations;

namespace Api_ELearning.Models
{
    public class Base
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public DateTime AddTime { get; set; } = DateTime.Now;
        public DateTime EditTime { get; set; } = DateTime.Now;
    }
}
