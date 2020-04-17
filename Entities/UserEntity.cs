using System;

namespace CommunicaptionBackend.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Pin { get; set; }
        public bool Connected { get; set; }
    }
}
