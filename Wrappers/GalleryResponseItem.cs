using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicaptionBackend.Wrappers {
    public class GalleryResponseItem {
        public string title { get; set; }
        public string text { get; set; }
        public int artId { get; set; }
        public byte[] picture { get; set; }
    }
}
