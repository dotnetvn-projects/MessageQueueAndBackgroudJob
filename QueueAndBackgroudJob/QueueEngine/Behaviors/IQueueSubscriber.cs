﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QueueEngine.Behaviors
{
    public interface IQueueSubscriber
    {
        Task ProcessQueue();
    }
}