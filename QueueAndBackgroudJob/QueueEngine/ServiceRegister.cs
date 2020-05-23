using Microsoft.Extensions.DependencyInjection;
using QueueEngine.Interfaces;
using QueueEngine.Models.QueueData;
using QueueEngine.Services;
using System;
using System.Collections.Generic;
using System.Text;

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
