using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lesson2ModelleringEntity.Producer
{
    public class AlbumProducers
    {
        [Required]
        public int AlbumID { get; set; }
        [Required]
        public int ProducerID { get; set; }
        public Album Album { get; set; }
        public Producer Producer { get; set; }
    }
}
