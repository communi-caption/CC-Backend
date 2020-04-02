using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunicaptionBackend.Api;
using CommunicaptionBackend.Core;
using CommunicaptionBackend.Entities;
using CommunicaptionBackend.Messages;

namespace CommunicaptionBackend.Api {

    public class MainService : IMainService {
        private readonly MainContext mainContext;
        private readonly MessageQueue messageQueue;

        public MainService(MainContext mainContext, MessageQueue messageQueue) {
            this.mainContext = mainContext;
            this.messageQueue = messageQueue;
        }

        public void DisconnectDevice(int userId) {
            var user = mainContext.Users.SingleOrDefault(x => userId == x.UserId);
            if (user == null)
                return;
            user.Connected = false;
            mainContext.SaveChanges();
        }

        public string GeneratePin() {
            var user = new UserEntity {
                Pin = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8),
                Connected = false
            };

            mainContext.Users.Add(user);
            mainContext.SaveChanges();
            return user.Pin;
        }

        public byte[] GetMediaData(string mediaId) {
            return File.ReadAllBytes("medias/" + mediaId);
        }

        public List<object> GetMediaItems(int userId)
        {
            List<object> itemInformations = new List<object>();
            var medias = mainContext.Medias.Where(x => x.UserId == userId);
            foreach (var media in medias)
            {
                object obj = new
                {
                    mediaId = media.MediaId,
                    fileName = Newtonsoft.Json.JsonConvert.SerializeObject(media.DateTime),
                    thumbnail = File.ReadAllBytes("thumbnails/" + media.MediaId.ToString()+ ".jpg")
                };
                itemInformations.Add(obj);
            }
            return itemInformations;         
        }

        public void PushMessage(Message message) {
            messageQueue.PushMessage(message);
        }

        public List<Message> GetMessages(int userId) {
            return messageQueue.SwapQueue(userId);
        }

        public int CheckForPairing(string pin) {
            var user = mainContext.Users.SingleOrDefault(x => x.Pin == pin);
            if (user == null)
                return 0;
            return user.UserId;
        }

        public bool CheckUserExists(int userId) {
            var user = mainContext.Users.SingleOrDefault(x => x.UserId == userId);
            if (user == null)
                return false;
            return true;
        }

        public int ConnectWithoutHoloLens() {
            UserEntity user = new UserEntity {
                Pin = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8),
                Connected = false
            };

            mainContext.Users.Add(user);
            mainContext.SaveChanges();

            return user.UserId;
        }

        public int ConnectWithHoloLens(string pin) {
            int userId = CheckForPairing(pin);

            if (userId == 0)
                return 0;
            else {
                var user = mainContext.Users.SingleOrDefault(x => userId == x.UserId);
                user.Connected = true;

                mainContext.Users.Update(user);
                mainContext.SaveChanges();

                return userId;
            }
        }
    }
}
