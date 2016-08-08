using System;
using System.Text;
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
            Logger.WriteInfo("trying to get the instance of connection manager");
            _rabbitMqchannel = ConnectionManager.Instance.ChannelInstance;
            _key = key;
            Logger.WriteInfo("connection manager is instanciated");
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
                    Logger.WriteInfo("retrieving the message from queue");
                    deliveryTag = ea.DeliveryTag;
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var basecommand = Serializer.DeserializeObject<T>(message);
                    ProcessMessage(basecommand);
                    Logger.WriteInfo("message processed");
                    _rabbitMqchannel.BasicAck(ea.DeliveryTag, false);
                    Logger.WriteInfo("Acknowledgement sent");
                };
                var messageProperty = MessagePropertyCollection.GetMessageProperty(_key);
                _rabbitMqchannel.BasicConsume(queue: messageProperty.QueueName,
                                     noAck: false,
                                     consumer: consumer);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex.ToString());
                _rabbitMqchannel.BasicNack(deliveryTag,false,false);
                throw;
            }
            

        }
    }
}
