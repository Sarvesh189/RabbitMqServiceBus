using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMqServiceBus.Utility;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqServiceBus
{
    public abstract class BaseCommandHandler
    {
        private readonly IModel _rabbitMqchannel;
        private readonly string _key;
       
        protected BaseCommandHandler(string key)
        {
            _rabbitMqchannel = ConnectionManager.Instance.ChannelInstance;
            this._key = key;
        }

        public abstract void ProcessMessage<T>(T command);
        public void Handle<T>()
        {
            ulong deliveryTag=100000;
            try
            {
                var consumer = new EventingBasicConsumer(_rabbitMqchannel);
                consumer.Received += (model, ea) =>
                {
                    deliveryTag = ea.DeliveryTag;
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var basecommand = Serializer.DeserializeObject<T>(message);
                    ProcessMessage(basecommand);
                    _rabbitMqchannel.BasicAck(ea.DeliveryTag, false);
                };
                var messageProperty = MessagePropertyCollection.GetMessageProperty(_key);
                _rabbitMqchannel.BasicConsume(queue: messageProperty.QueueName,
                                     noAck: false,
                                     consumer: consumer);
            }
            catch (Exception)
            {
                _rabbitMqchannel.BasicNack(deliveryTag,false,false);
                throw;
            }
            

        }
    }
}
