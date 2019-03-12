using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lesson2ModelleringEntity.Producer
{
    public class Producer
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public List<AlbumProducers> AlbumProducers { get; set; }
    }
}
