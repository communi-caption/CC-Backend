using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicaptionBackend.Entities;

namespace CommunicaptionBackend.Messages
{
    public class SaveMediaMessage : Message
    {
        public int UserID { get; set; }
        public int FileSize { get; set; }
        public bool MediaType { get; set; }
        public byte[] Data { get; set; }
    }
}
