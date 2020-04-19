using System;
using System.Collections.Generic;

namespace CommunicaptionBackend.Entities {

    public class ArtEntity {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
