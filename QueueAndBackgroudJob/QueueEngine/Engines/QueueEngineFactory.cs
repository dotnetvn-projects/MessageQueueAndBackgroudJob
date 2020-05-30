using QueueEngine.Behaviors;
using QueueEngine.Engines.Google;
using QueueEngine.Models.QueueSetting;
using System;

namespace QueueEngine
{
    public class QueueEngineFactory
    {
        public static IQueuePublisher<T> CreateGooglePublisher<T>(QueueProvider provider, QueueSetting queueSetting, string topicName)
        {
            IQueuePublisher<T> publisher = default;
            switch (provider)
            {
                case QueueProvider.GOOGLE:
                    publisher = new GoogleQueuePublisher<T>(queueSetting, topicName);
                    break;
            }
            return publisher;
        }

        public static IQueueSubscriber CreateGoogleSubscriber(QueueProvider provider, QueueSetting queueSetting, string subscriptionName, Action<string> handler)
        {
            IQueueSubscriber subscriber = default;
            switch (provider)
            {
                case QueueProvider.GOOGLE:
                    subscriber = new GoogleQueueSubscriber(queueSetting, subscriptionName, handler);
                    break;
            }
            return subscriber;
        }
    }
}