using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicaptionBackend.Entities;

namespace CommunicaptionBackend.Messages {

    public class SaveTextMessage : Message {
        public byte[] Data { get; set; }
    }
}
