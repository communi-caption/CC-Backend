using CommunicaptionBackend.Api;
using CommunicaptionBackend.Models;
using CommunicaptionBackend.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicaptionBackend.Core
{
    public class MessageProcessor
    {
        private MainContext mediaContext;

        public void SaveMedia(SaveMediaMessage message)
        {
            Media media = new Media();
            media.type = message.MediaType;
            media.userId = message.UserID;
            media.size = message.FileSize;
            media.dateTime = DateTime.Now;
            mediaContext.Medias.Add(media);
            mediaContext.SaveChanges();


        }
    }
}
