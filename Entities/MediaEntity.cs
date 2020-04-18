using System;
using System.Diagnostics.CodeAnalysis;

namespace CommunicaptionBackend.Entities
{
    public class MediaEntity
    {
        public int UserId { get; set; }
        public int ArtId { get; set; }
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public bool Type { get; set; }
        public int Size { get; set; }
    }
}
