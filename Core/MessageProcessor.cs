using CommunicaptionBackend.Api;
using CommunicaptionBackend.Entities;
using CommunicaptionBackend.Messages;
using System;
using System.IO;
using System.Threading.Tasks;
using Xabe.FFmpeg.Model;
using Xabe.FFmpeg;
using Newtonsoft.Json;

namespace CommunicaptionBackend.Core
{
    public class MessageProcessor
    {
        private MainContext mediaContext;

        public MessageProcessor(MainContext mediaContext)
        {
            this.mediaContext = mediaContext;
        }

        public void ProcessMessage(Message message)
        {
            switch (message)
            {
                case SaveMediaMessage saveMediaMessage:
                    Task.Run(async () => await ByteToFileAsync(saveMediaMessage));
                    break;
                case SettingsChangedMessage settingsChangedMessage:
                    SaveSettingsToDB(settingsChangedMessage);
                    break;
                default:
                    throw new Exception("Bug found!");
            }
        }

        private async Task ByteToFileAsync(SaveMediaMessage message)
        {
            MediaEntity media = new MediaEntity();
            media.Type = message.MediaType;
            media.UserId = message.UserId;
            media.Size = message.FileSize;
            media.DateTime = DateTime.Now;
            mediaContext.Medias.Add(media);
            mediaContext.SaveChanges();

            string filename = media.Id.ToString();
            string saveImagePath = ("medias/") + filename;
            File.WriteAllBytes(saveImagePath, message.Data);

            string saveThumbnPath = ("thumbnails/") + filename + ".jpg";
            IConversionResult result = await Conversion.Snapshot("saveImagePath", saveThumbnPath, TimeSpan.FromSeconds(0)).Start();
        }

        private void SaveSettingsToDB(SettingsChangedMessage message)
        {
            SettingsEntity settings = new SettingsEntity();
            settings.UserId = message.UserId;

            string result = JsonConvert.SerializeObject(message);
            settings.Json = result;

            mediaContext.Settings.Add(settings);
            mediaContext.SaveChanges();
        }
    }
}
