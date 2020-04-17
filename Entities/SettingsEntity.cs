using System;

namespace CommunicaptionBackend.Entities
{
    public class SettingsEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Json { get; set; }
    }
}
