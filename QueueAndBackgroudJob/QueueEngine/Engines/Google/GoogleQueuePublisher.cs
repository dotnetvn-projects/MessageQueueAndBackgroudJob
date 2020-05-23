﻿using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Grpc.Auth;
using Newtonsoft.Json;
using QueueEngine.Behaviors;
using QueueEngine.Models.QueueSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace QueueEngine.Engines.Google
{
    class GoogleQueuePublisher<T> : IQueuePublisher<T>
    {
        private PublisherClient _publisher;
        private readonly GoogleQueueSetting _queueSetting;
        private SubscriptionName _subscriptionName;
        private TopicName _topicName;
        public GoogleQueuePublisher(QueueSetting queueSetting, string topicName)
        {
            _queueSetting = queueSetting as GoogleQueueSetting;
            InitializeQueue(topicName);
        }

        private void InitializeQueue(string topicName)
        {
            if (_publisher == null)
            {
                var googleCredential = GoogleCredential.FromFile(_queueSetting.CredentialFile);
                var createSettings = new PublisherClient.ClientCreationSettings(credentials: googleCredential.ToChannelCredentials());
                var toppicName = new TopicName(_queueSetting.ProjectId, topicName);
                var publisher = PublisherClient.CreateAsync(toppicName, createSettings);
                _publisher = publisher.Result;

                //_publisher = PublisherServiceApiClient.Create();
              //  _topicName = new TopicName(_queueSetting.ProjectId, topicName);
            }
        }

        public async Task SendMessage(T message)
        {
            var body = JsonConvert.SerializeObject(message);

            var attributes = new Dictionary<string, string>
            {
                { "Message.Type.FullName", message.GetType().FullName }
            };

            var pubsubMessage = new PubsubMessage()
            {
                Data = ByteString.CopyFromUtf8(body)
            };

            pubsubMessage.Attributes.Add(attributes);

            var messageResponse = await _publisher.PublishAsync(pubsubMessage);
        }

        public async Task SendMessages(IList<T> messages)
        {
            var publishTasks =
               messages.Select(async message =>
               {
                   await SendMessage(message);
               });
            await Task.WhenAll(publishTasks);
        }
    }
}
