using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QueueEngine;
using QueueEngine.Behaviors;
using QueueEngine.Interfaces;
using QueueEngine.Models.QueueData;
using QueueEngine.Models.QueueSetting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmailService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IQueueSubscriber _subscriber;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _subscriber = CreateSubscriber();
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _subscriber.ProcessQueue();
        }

        private IQueueSubscriber CreateSubscriber()
        {
            var providerSetting = _configuration["MessageQueueSetting:Provider"];
            var provider = Common.ParseEnum<QueueProvider>(providerSetting);

            switch (provider)
            {
                case QueueProvider.GOOGLE:

                    var googleSetting = new GoogleQueueSetting();
                    var settingPath = _configuration.GetSection("MessageQueueSetting:GoogleQueueSetting");
                    settingPath.Bind(googleSetting);

                    return QueueEngineFactory.CreateGoogleSubscriber(provider, googleSetting,
                        "EmailQueue", "EmailQueueSub", MesageHandler);
            }

            return default;
        }

        public void MesageHandler(string body)
        {
            var data = JsonConvert.DeserializeObject<OrderQueue>(body);
            Console.WriteLine(@$"{DateTime.Now} Send email for order: OrderId={data.OrderId}, Customer={data.CustomerName}, ProductId={data.ProductId}");
        }
    }
}
