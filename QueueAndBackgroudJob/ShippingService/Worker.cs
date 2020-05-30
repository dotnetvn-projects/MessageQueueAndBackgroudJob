using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QueueEngine;
using QueueEngine.Behaviors;
using QueueEngine.Models.QueueData;
using QueueEngine.Models.QueueSetting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingService
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
            if (_subscriber != null)
            {
                await _subscriber.ProcessQueue();
            }
        }

        private IQueueSubscriber CreateSubscriber()
        {
            try
            {
                var providerSetting = _configuration["MessageQueueSetting:Provider"];
                var provider = Common.ParseEnum<QueueProvider>(providerSetting);

                switch (provider)
                {
                    case QueueProvider.GOOGLE:

                        var googleSetting = new GoogleQueueSetting();
                        var settingPath = _configuration.GetSection("MessageQueueSetting:GoogleQueueSetting");
                        settingPath.Bind(googleSetting);

                        return QueueEngineFactory.CreateGoogleSubscriber(provider, googleSetting, "ShippingQueueSub", MesageHandler);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return default;
        }

        public void MesageHandler(string body)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<OrderQueue>(body);
                Console.WriteLine(@$"{DateTime.Now} Prepare shipping for order: OrderId={data.OrderId}, Customer={data.CustomerName}, ProductId={data.ProductId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}