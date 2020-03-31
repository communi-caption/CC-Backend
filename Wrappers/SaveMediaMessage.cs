using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicaptionBackend.Models;

namespace CommunicaptionBackend.Wrappers
{
    public class SaveMediaMessage
    {
        public int UserID { get; set; }
        public int FileSize { get; set; }
        public bool MediaType;
        public byte[] Data;
    }
}
