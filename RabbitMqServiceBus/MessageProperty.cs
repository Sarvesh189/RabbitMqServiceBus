using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqServiceBus
{
  public  class MessageProperty
    {
        public string ExchangeName { get; set; }

        public string ExchangeType { get; set; }

        public string QueueName { get; set; }

        public bool Durable { get; set; }

        public string RoutingKey { get;set; }
    }

    public class MessagePropertyCollection
    {
        private  static readonly Dictionary<string, MessageProperty> MessageProperties =new Dictionary<string, MessageProperty>();

        public static void AddMessageProperty(string key, MessageProperty messageProperty)
        {
            MessageProperties.Add(key, messageProperty);
        }

        public static MessageProperty GetMessageProperty(string key)
        {
            MessageProperty messageProperty;
            if (!MessageProperties.TryGetValue(key, out messageProperty))
            {
                throw new ArgumentException($"Key: {key} does not exist in configuration please recheck configuration");
            }
            return messageProperty;

        }

    }
}
