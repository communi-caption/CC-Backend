using CommunicaptionBackend.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicaptionBackend.Core {

    public class MessageQueue {

        public ConcurrentQueue<Message> messages;

        public MessageQueue() {
            this.messages = new ConcurrentQueue<Message>();
        }

        public void PushMessage(Message message) {
            messages.Enqueue(message);
        }
        public List<Message> GetMessages(int userId)
        {
            List<Message> message_list = new List<Message>();
            foreach (var message in messages)
            {
                if (message.UserID == userId)
                    message_list.Add(message);


            }
            return message_list;
        }

        public bool TryPopMessage(out Message message) {
            return messages.TryDequeue(out message);
        }
    }
}
