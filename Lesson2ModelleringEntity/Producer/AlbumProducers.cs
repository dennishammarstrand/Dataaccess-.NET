using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lesson2ModelleringEntity.Producer
{
    class AlbumProducers
    {
        [Required]
        public int AlbumID { get; set; }
        [Required]
        public int ProducerID { get; set; }
    }
}
