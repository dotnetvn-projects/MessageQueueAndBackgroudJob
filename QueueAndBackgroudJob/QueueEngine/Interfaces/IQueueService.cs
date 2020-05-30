using System.Collections.Generic;

namespace QueueEngine.Interfaces
{
    public interface IQueueService<T>
    {
        void PushQueue(T model);

        IList<T> ConsumeQueue(int lenght);
    }
}