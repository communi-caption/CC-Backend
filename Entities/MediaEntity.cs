using System;

namespace CommunicaptionBackend.Entities
{
    public class MediaEntity
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public DateTime DateTime{ get; set; }
        public bool Type { get; set; }
        public int Size { get; set; }
    }
}
