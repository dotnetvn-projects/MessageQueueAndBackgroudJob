using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace QueueEngine.Interfaces
{
    public interface IQueueService<T>
    {
        void PushQueue(T model);

        IList<T> ConsumeQueue(int lenght);
    }
}
