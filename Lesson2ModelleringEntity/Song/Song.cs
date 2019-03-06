using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lesson2ModelleringEntity
{
    public class Song
    {
        [Key]
        public int ID { get; set; }
        public int TrackNumber { get; set; }
        public string Title { get; set; }
        public Int16 Length { get; set; }
        public bool HasMusicVideo { get; set; }
        public string Lyrics { get; set; }
        public Album Album { get; set; }
    }
}
