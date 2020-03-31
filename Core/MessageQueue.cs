using CommunicaptionBackend.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicaptionBackend.Core
{
    public class MessageQueue
    {
        public static List<SaveMediaMessage> messages;

        public void PushMessage(string userId, SaveMediaMessage message)
        {
            if (messages.Count == 0)
                messages = new List<SaveMediaMessage>();

            messages.Add(message);
        }
        

    }
}
