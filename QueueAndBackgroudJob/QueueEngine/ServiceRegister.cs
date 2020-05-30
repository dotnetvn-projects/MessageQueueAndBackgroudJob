using QueueEngine.Interfaces;
using QueueEngine.Models.QueueData;
using QueueEngine.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceRegister
    {
        public static void AddQueueService(this IServiceCollection services)
        {
            services.AddSingleton<IQueueService<OrderQueue>, QueueService<OrderQueue>>();
        }
    }
}