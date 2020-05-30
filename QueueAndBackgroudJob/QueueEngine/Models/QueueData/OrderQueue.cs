using System;

namespace QueueEngine.Models.QueueData
{
    public class OrderQueue
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public Guid ProductId { get; set; }
    }
}