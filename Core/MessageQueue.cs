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
        public ConcurrentQueue<Message> GetMessages()
        {
            return messages;
        }

        public bool TryPopMessage(out Message message) {
            return messages.TryDequeue(out message);
        }
    }
}
