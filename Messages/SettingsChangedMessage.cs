using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicaptionBackend.Messages
{
    public class SettingsChangedMessage : Message
    {
        public string Mode { get; set; }
        public string NativeLanguageCode { get; set; }
        public string ForeignLanguageCode { get; set; }
        public string SubtitleTrigger { get; set; }
        public string TranslateLanguage { get; set; }
    }
}
