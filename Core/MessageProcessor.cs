using CommunicaptionBackend.Api;
using CommunicaptionBackend.Entities;
using CommunicaptionBackend.Messages;
using System;
using System.IO;
using System.Threading.Tasks;
using Xabe.FFmpeg.Model;
using Xabe.FFmpeg;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace CommunicaptionBackend.Core
{
    public class MessageProcessor
    {
        private MainContext mainContext;

        public MessageProcessor(MainContext mainContext)
        {
            this.mainContext = mainContext;
        }

        public void ProcessMessage(Message message)
        {
            switch (message)
            {
                case SaveMediaMessage saveMediaMessage:
                    ProcessMedia(saveMediaMessage);
                    break;
                case SaveTextMessage saveTextMessage:
                    ProcessText(saveTextMessage);
                    break;
                case SettingsChangedMessage settingsChangedMessage:
                    SaveSettingsToDB(settingsChangedMessage);
                    break;
                default:
                    throw new Exception("Bug found!");
            }
        }

        private void ProcessMedia(SaveMediaMessage message)
        {
            if (!Directory.Exists("medias"))
                Directory.CreateDirectory("medias");
            if (!Directory.Exists("thumbnails"))
                Directory.CreateDirectory("thumbnails");

            var media = new MediaEntity();
            media.Type = message.MediaType;
            media.UserId = message.UserId;
            media.Size = message.FileSize;
            media.DateTime = DateTime.Now;
            mainContext.Medias.Add(media);
            mainContext.SaveChanges();

            string filename = media.Id.ToString();
            string saveImagePath = ("medias/") + filename;
            File.WriteAllBytes(saveImagePath, message.Data);

            string saveThumbnPath = ("thumbnails/") + filename + ".jpg";
            File.WriteAllBytes(saveThumbnPath, message.Data);
        }

        private void ProcessText(SaveTextMessage message) {
            if (!Directory.Exists("texts"))
                Directory.CreateDirectory("texts");
            if (!Directory.Exists("texts"))
                Directory.CreateDirectory("texts");

            string text = MakeOCR(message.Data);
            if (string.IsNullOrWhiteSpace(text)) {
                return;
            }

            var textEntity = new TextEntity();
            textEntity.UserId = message.UserId;
            textEntity.DateTime = DateTime.Now;
            textEntity.Text = text;

            mainContext.Texts.Add(textEntity);
            mainContext.SaveChanges();

            string filename = textEntity.Id.ToString();
            string saveTextPath = "texts/" + filename + ".txt";
            File.WriteAllText(saveTextPath, text);
        }

        private string MakeOCR(byte[] image) {
            string uri = "https://westus.api.cognitive.microsoft.com/vision/v2.0/ocr?language=unk&detectOrientation=true";

            var web = new WebClient();
            web.Proxy = null;
            web.Headers["Ocp-Apim-Subscription-Key"] = "a71411081181417d95a272965a6f52a0";
            web.Headers["Content-Type"] = "application/octet-stream";

            var res = web.UploadData(uri, "POST", image);
            var json = JObject.Parse(Encoding.UTF8.GetString(res));
            var regions = json["regions"] as JArray;

            return string.Join("\n\n", regions.Select(x => {
                if (!(x["lines"] is JArray lines)) return null;
                return string.Join("\n", lines.Select(y => {
                    if (!(y["words"] is JArray words)) return null;
                    return string.Join(" ", words.Select(w => w["text"].ToString())
                        .Where(z => z != null));
                }).Where(y => y != null));
            }).Where(x => x != null));
        }

        private void SaveSettingsToDB(SettingsChangedMessage message)
        {
            var settings = mainContext.Settings.FirstOrDefault();
            if (settings == null) {
                settings = new SettingsEntity();
                settings.UserId = message.UserId;
                settings.Json = JsonConvert.SerializeObject(message);
                mainContext.Settings.Add(settings);
            } else {
                settings.UserId = message.UserId;
                settings.Json = JsonConvert.SerializeObject(message);
                mainContext.Settings.Update(settings);
            }
            mainContext.SaveChanges();
        }
    }
}
