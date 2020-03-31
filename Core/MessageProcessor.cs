using CommunicaptionBackend.Api;
using CommunicaptionBackend.Entities;
using CommunicaptionBackend.Messages;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicaptionBackend.Core
{
    public class MessageProcessor
    {
        private MainContext mediaContext;

        public MessageProcessor(MainContext mediaContext)
        {
            this.mediaContext = mediaContext;
        }

        public void ByteToFile(SaveMediaMessage message)
        {
            MediaEntity media = new MediaEntity();
            media.Type = message.MediaType;
            media.UserId = message.UserID;
            media.Size = message.FileSize;
            media.DateTime = DateTime.Now;
            mediaContext.Medias.Add(media);
            mediaContext.SaveChanges();

            if (media.Type.Equals("photo"))
            {
                string filename = media.MediaId.ToString();
                string saveImagePath = ("medias/") + filename + ".png";
                File.WriteAllBytes(saveImagePath, message.Data);
            }
            else if(media.Type.Equals("video"))
            {
                string filename = media.MediaId.ToString();
                string saveImagePath = ("medias/") + filename + ".mp4";
                File.WriteAllBytes(saveImagePath, message.Data);
            }
            else
            {
                string filename = media.MediaId.ToString();
                string saveImagePath = ("thumbnails/") + filename + ".png";
                File.WriteAllBytes(saveImagePath, message.Data);
            }
        }
        
    }
}
