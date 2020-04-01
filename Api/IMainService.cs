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

        public List<Message> GetMessages();

        public void DisconnectDevice(int userId);

        public string CheckForPairing(string pin);

        public bool CheckUserExists(int userId);

        public string ConnectWithoutHoloLens();
        
        public int ConnectWithHoloLens(string pin);
    }
}