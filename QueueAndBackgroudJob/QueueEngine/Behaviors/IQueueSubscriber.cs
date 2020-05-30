using System.Threading.Tasks;

namespace QueueEngine.Behaviors
{
    public interface IQueueSubscriber
    {
        Task ProcessQueue();
    }
}