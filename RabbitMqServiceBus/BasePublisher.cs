using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RabbitMqServiceBus.Utility;
using RabbitMQ.Client;
using RabbitMqServiceBus;

namespace RabbitMqServiceBus
{
    internal interface IPublisher
    {
        void Publish<T>(T baseCommand);
    }

    public class BasePublisher: IPublisher
    {
       private readonly IModel _rabbitMqchannel;

      
       public BasePublisher()
       {
           _rabbitMqchannel = ConnectionManager.Instance.ChannelInstance;

       }

        public void Publish<T>(T command)
        {
            var commandMetaDataAttribute = command.GetType().GetCustomAttribute<CommandMetaDataAttribute>();
            var messageProperty = MessagePropertyCollection.GetMessageProperty(commandMetaDataAttribute.Key);
            string strcommand = Serializer.SerializeObject(command);
            var body = Encoding.UTF8.GetBytes(strcommand);
            var properties = _rabbitMqchannel.CreateBasicProperties();
            properties.Persistent = true;
            properties.DeliveryMode = 2;
            _rabbitMqchannel.BasicPublish(messageProperty.ExchangeName, messageProperty.RoutingKey, true, properties, body);
        }
    }
}
