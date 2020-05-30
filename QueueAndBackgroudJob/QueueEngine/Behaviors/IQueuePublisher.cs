using System.Collections.Generic;
using System.Threading.Tasks;

namespace QueueEngine.Behaviors
{
    public interface IQueuePublisher<T>
    {
        Task SendMessage(T message);

        Task SendMessages(IList<T> messages);
    }
}