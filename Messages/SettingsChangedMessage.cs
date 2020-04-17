using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicaptionBackend.Messages
{
    public class SettingsChangedMessage : Message
    {
        public string Mode { get; set; } = "0";
        public string NativeLanguageCode { get; set; } = "tr";
        public string ForeignLanguageCode { get; set; } = "en";
        public string SubtitleTrigger { get; set; } = "0";
        public string TranslateLanguage { get; set; } = "0";
    }
}
