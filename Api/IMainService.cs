using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

using CommunicaptionBackend.Messages;

namespace CommunicaptionBackend.Api {

    public interface IMainService {

        string GeneratePin();

        byte[] GetMediaData(string mediaId);

        public void PushMessage(Message message);

        public ConcurrentQueue<Message> GetMessages();

        public void DisconnectDevice(string userId);

        public string CheckForPairing(string pin);

        public bool CheckUserExists(string UserId);

        public string ConnectWithoutHoloLens();
        
        public string ConnectWithHoloLens(string pin);
    }
}