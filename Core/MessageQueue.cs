using CommunicaptionBackend.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicaptionBackend.Core {

    public class MessageQueue {

        public ConcurrentDictionary<int, ConcurrentQueue<Message>> queues;

        public MessageQueue() {
            this.queues = new ConcurrentDictionary<int, ConcurrentQueue<Message>>();
        }

        public void PushMessage(Message message) {
            var queue = queues.GetOrAdd(message.UserId, new ConcurrentQueue<Message>());
            queue.Enqueue(message);
        }

        public bool TryPopMessage(int userId, out Message message) {
            var queue = queues.GetOrAdd(userId, new ConcurrentQueue<Message>());
            return queue.TryDequeue(out message);
        }

        public List<Message> SwapQueue(int userId) {
            var res = new List<Message>();
            Message message;
            while (TryPopMessage(userId, out message))
                res.Add(message);
            return res;
        }
    }
}
