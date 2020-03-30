using CommunicaptionBackend.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicaptionBackend.Core
{
    public class MessageProcessor
    {
        public static List<Message> messages;

        public void PushMessage(string userId, Message message)
        {
            if (messages.Count == 0)
                messages = new List<Message>();

            messages.Add(message);
        }
        

    }
}
