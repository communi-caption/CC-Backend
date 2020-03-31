using CommunicaptionBackend.Api;
using CommunicaptionBackend.Entities;
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
            MediaEntity media = new MediaEntity();
            media.Type = message.MediaType;
            media.UserId = message.UserID;
            media.Size = message.FileSize;
            media.DateTime = DateTime.Now;
            mediaContext.Medias.Add(media);
            mediaContext.SaveChanges();


        }
    }
}
