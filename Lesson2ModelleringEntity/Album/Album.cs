using Lesson2ModelleringEntity.Producer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lesson2ModelleringEntity
{
    public class Album
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        [Required]
        public Artist Artist{ get; set; }
        public List<Song> Songs { get; set; }
        public List<AlbumProducers> AlbumProducers { get; set; }
    }
}
