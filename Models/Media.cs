using System;
namespace CommunicaptionBackend.Models
{
    public class Media
    {
        public string userId { get; set; }
        public string mediaId { get; set; }
        public User user { get; set; }
        public DateTime dateTime{ get; set; }
        public string type { get; set; }



    }
}
