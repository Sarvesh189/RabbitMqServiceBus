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
          
           int ttl = 20; //default time in minute to live
            foreach (RabbitMqBusEndpointElement element in _rabbitMqsection.ConnectionManagerEndpoints)
            {
                if (!string.IsNullOrWhiteSpace(element.Name))
                {
                    MessagePropertyCollection.AddMessageProperty(element.Name, new MessageProperty() { ExchangeName = element.Exchange, ExchangeType  = element.ExchangeType ,Durable = bool.Parse(element.Durable), QueueName = element.Queue, RoutingKey = element.Routingkey });
                    channel.ExchangeDeclare(element.Exchange, element.ExchangeType, true);
                    int timetolive;

                    if (int.TryParse(element.TTL, out timetolive))
                    {
                        ttl = timetolive*60*1000; //convert to miliseconds
                    }
                    else
                    {
                        ttl = ttl*60*1000; //convert to  miliseconds
                    }


                    var args = new Dictionary<string, object>();
                    if (ttl > 0)
                    {
                        args.Add("x-message-ttl", ttl);
                    }
                    args.Add("x-ha-policy","all");
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
