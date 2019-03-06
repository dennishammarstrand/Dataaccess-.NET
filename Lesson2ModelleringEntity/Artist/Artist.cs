using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lesson2ModelleringEntity
{
    public class Artist
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public Int16 YearStarted { get; set; }
        public List<Album> Albums { get; set; }
    }
}
