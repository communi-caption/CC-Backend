using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicaptionBackend.Api;
using CommunicaptionBackend.Messages;

namespace CommunicaptionBackend.Api {

    public class MainService : IMainService {
        private readonly MainContext mainContext;

        public MainService(MainContext mainContext) {
            this.mainContext = mainContext;
        }

        public void DisconnectDevice(string userId) {
            // todo
        }

        public string GeneratePin() {
            return Guid.NewGuid().ToString();
        }

        public byte[] GetMediaData(string mediaId) {
            return new byte[0]; // todo
        }

        public void PushMessage(Message message) {
            // todo
        }

        public string CheckForPairing(string pin) {
            return null; // todo
        }

        public string ConnectWithoutHoloLens() {
            return null; // todo
        }

        public string ConnectWithHoloLens(string pin) {
            string userId = CheckForPairing(pin);
            // todo
            return userId;
        }
    }
}
