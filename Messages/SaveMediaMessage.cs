using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicaptionBackend.Models;

namespace CommunicaptionBackend.Messages
{
    public class SaveMediaMessage : Message
    {
        public int UserID { get; set; }
        public int FileSize { get; set; }
        public bool MediaType;
        public byte[] Data;
    }
}
