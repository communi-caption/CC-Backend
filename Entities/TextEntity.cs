using System;

namespace CommunicaptionBackend.Entities {

    public class TextEntity {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string Text { get; set; }
    }
}
