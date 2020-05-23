﻿using Google.Api.Gax;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Grpc.Auth;
using Grpc.Core;
using QueueEngine.Behaviors;
using QueueEngine.Models.QueueSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QueueEngine.Engines.Google
{
    class GoogleQueueSubscriber : IQueueSubscriber
    {
        private SubscriberClient _subscriber;
        private readonly GoogleQueueSetting _queueSetting;
        private SubscriptionName _subscriptionName;
        private readonly Action<string> _messageHandler;

        public GoogleQueueSubscriber(QueueSetting queueSetting, string topicName, string subscriptionName, Action<string> messageHandler)
        {
            _queueSetting = queueSetting as GoogleQueueSetting;
            _messageHandler = messageHandler;
            InitializeQueue(topicName, subscriptionName);
        }

        private void InitializeQueue(string topicName, string subscriptionName)
        {
            if (_subscriber == null)
            {
                _subscriptionName = new SubscriptionName(_queueSetting.ProjectId, subscriptionName);
                var googleCredential = GoogleCredential.FromFile(_queueSetting.CredentialFile);
                var createSettings = new SubscriberClient.ClientCreationSettings(credentials: googleCredential.ToChannelCredentials());
                var subscriber = SubscriberClient.CreateAsync(_subscriptionName, createSettings);
                _subscriber = subscriber.Result;
            }
        }

        /// <summary>
        /// Process message queue
        /// </summary>
        /// <returns></returns>
        public async Task ProcessQueue()
        {
            await _subscriber.StartAsync(
                 (PubsubMessage message, CancellationToken cancel) =>
                {
                    string body = Encoding.UTF8.GetString(message.Data.ToArray());

                    _messageHandler(body);

                    return Task.FromResult(SubscriberClient.Reply.Ack);
                });
        }
    }
}
