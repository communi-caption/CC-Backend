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

        public byte[] getSearchResult(string json);

        public List<object> GetMediaItems(int userId);

        public void PushMessage(Message message);

        public List<Message> GetMessages(int userId);

        public void DisconnectDevice(int userId);

        public int CheckForPairing(string pin);

        public bool CheckUserExists(int userId);

        public int ConnectWithoutHoloLens();
        
        public int ConnectWithHoloLens(string pin);

        public string GetUserSettings(int userId);
    }
}