using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMqServiceBus.Configuration;
using RabbitMQ.Client;

namespace RabbitMqServiceBus
{
    internal interface IBrokerSetup
    {
        void SetupExchangeAndQueue(IModel channel);
    }


    internal class BrokerSetup: IBrokerSetup
    {
      
        private readonly RabbitMqBusConfigurationSection _rabbitMqsection;
        public BrokerSetup()
       {
           
            _rabbitMqsection = (RabbitMqBusConfigurationSection)ConfigurationManager.GetSection("RabbitMqBusConfigurationSection");
        }

       public void SetupExchangeAndQueue(IModel channel)
       {
           SetUpDeadLetterExchange(channel);
           SetUpNormalExchanges(channel);
       }

       private void SetUpDeadLetterExchange(IModel channel)
       {
            channel.ExchangeDeclare(_rabbitMqsection.DeadLetterExchange.Exchange, _rabbitMqsection.DeadLetterExchange.ExchangeType, true);
            
       }


       private void SetUpNormalExchanges(IModel channel)
       {
            foreach (RabbitMqBusEndpointElement element in _rabbitMqsection.ConnectionManagerEndpoints)
            {
                if (!string.IsNullOrWhiteSpace(element.Name))
                {
                    MessagePropertyCollection.AddMessageProperty(element.Name, new MessageProperty() { ExchangeName = element.Exchange, ExchangeType  = element.ExchangeType ,Durable = bool.Parse(element.Durable), QueueName = element.Queue, RoutingKey = element.Routingkey });
                    channel.ExchangeDeclare(element.Exchange, element.ExchangeType, true);
                    var ttl = int.Parse(element.TTL);
                    var args = new Dictionary<string, object>();
                    if (ttl > 0)
                    {
                         ttl = int.Parse(element.TTL);
                        args.Add("x-message-ttl", ttl);

                    }

                    if (element.DeadletterRouting)
                    {
                        args.Add("x-dead-letter-exchange", _rabbitMqsection.DeadLetterExchange.Exchange);
                    }

                    channel.QueueDeclare(element.Queue, true, false, false, args);
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                    channel.QueueBind(element.Queue, element.Exchange, element.Routingkey);
                }
            }
        }
   }
}
