using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunicaptionBackend.Api;
using CommunicaptionBackend.Core;
using CommunicaptionBackend.Entities;
using CommunicaptionBackend.Messages;

namespace CommunicaptionBackend.Api
{

    public class MainService : IMainService
    {
        private readonly MainContext mainContext;
        private readonly MessageQueue messageQueue;

        public MainService(MainContext mainContext, MessageQueue messageQueue)
        {
            this.mainContext = mainContext;
            this.messageQueue = messageQueue;
        }

        public void DisconnectDevice(string userId)
        {
            var user = mainContext.Users.SingleOrDefault(x => userId == x.UserId);
            if (user == null)
                return;

            user.Connected = false;

            mainContext.SaveChanges();
        }

        public string GeneratePin()
        {
            UserEntity user = new UserEntity
            {
                Pin = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8),
                Connected = false
            };

            mainContext.Users.Add(user);
            mainContext.SaveChanges();

            return user.Pin;
        }

        public byte[] GetMediaData(string mediaId)
        {
            return File.ReadAllBytes("medias/" + mediaId);
        }

        public void PushMessage(Message message)
        {
            messageQueue.PushMessage(message);
        }

        public string CheckForPairing(string pin)
        {
            var user = mainContext.Users.SingleOrDefault(x => x.Pin == pin);
            if (user == null)
                return null;

            return user.UserId;
        }

        public string ConnectWithoutHoloLens()
        {
            UserEntity user = new UserEntity
            {
                Pin = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8),
                Connected = false
            };

            mainContext.Users.Add(user);
            mainContext.SaveChanges();

            return user.Pin; 
        }

        public string ConnectWithHoloLens(string pin)
        {
            string userId = CheckForPairing(pin);

            if (userId == null)
                return null;
            else
            {
                var user = mainContext.Users.SingleOrDefault(x => userId == x.UserId);
                user.Connected = true;

                mainContext.Users.Update(user);
                mainContext.SaveChanges();

                return userId;
            }
                
        }
    }
}
