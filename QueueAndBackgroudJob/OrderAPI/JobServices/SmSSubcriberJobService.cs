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

namespace OrderAPI.JobServices
{
    public class SmSSubcriberJobService : IHostedService
    {
        private readonly ILogger<SmSSubcriberJobService> _logger;
        private readonly IQueueSubscriber _subscriber;
        private readonly IConfiguration _configuration;

        public SmSSubcriberJobService(ILogger<SmSSubcriberJobService> logger,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _subscriber = CreateSubscriber();
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                if (_subscriber != null)
                {
                    await _subscriber.ProcessQueue();
                }
            }, cancellationToken);
          
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
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

                        return QueueEngineFactory.CreateGoogleSubscriber(provider, googleSetting, "SmSQueueSub", MesageHandler);
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
                Console.WriteLine(@$"{DateTime.Now} Send SmS for order: OrderId={data.OrderId}, Customer={data.CustomerName}, ProductId={data.ProductId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
