using System;
namespace CommunicaptionBackend.Models
{
    public class Media
    {
        public int userId { get; set; }
        public int mediaId { get; set; }
        public DateTime dateTime{ get; set; }
        public bool type { get; set; }
        public int size { get; set; }



    }
}
