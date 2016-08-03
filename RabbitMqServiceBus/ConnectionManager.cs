using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMqServiceBus.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;

namespace RabbitMqServiceBus
{
  internal  class ConnectionManager
    {
        private IConnection _rabbitmqConnection;
        private IModel _channel;
        private static readonly ConnectionManager PrivateInstance = new ConnectionManager();

        #region URI detail

       // private int port = 5672;
        private readonly IProtocol _protocol = Protocols.AMQP_0_9_1;
     //   private bool automaticRecovery = true;
       
      
      private readonly RabbitMqBusConfigurationSection _rabbitMqsection;
      private readonly IBrokerSetup _brokerSetup = new BrokerSetup();

        #endregion

        public static ConnectionManager Instance
        {
            get { return PrivateInstance; }
        }
        public IModel ChannelInstance {
            get { return _channel; }
        }

        private ConnectionManager()
        {
             _rabbitMqsection = (RabbitMqBusConfigurationSection)ConfigurationManager.GetSection("RabbitMqBusConfigurationSection");
           
            InitializeSetup();
        }

        private IConnection OpenConnection()
        {
           
               var connectionFactory = new ConnectionFactory();
                connectionFactory.AutomaticRecoveryEnabled = true;
                connectionFactory.HandshakeContinuationTimeout =TimeSpan.FromMinutes(int.Parse(_rabbitMqsection.RabbitMqConnection.ConnectionTimeout));
                connectionFactory.ContinuationTimeout = TimeSpan.FromMinutes(int.Parse(_rabbitMqsection.RabbitMqConnection.ConnectionTimeout));
                connectionFactory.UserName = _rabbitMqsection.RabbitMqConnection.UserName;
                connectionFactory.Password = _rabbitMqsection.RabbitMqConnection.Password;
                connectionFactory.Port = int.Parse(_rabbitMqsection.RabbitMqConnection.Port);
                connectionFactory.Protocol = _protocol;
                connectionFactory.VirtualHost = _rabbitMqsection.RabbitMqConnection.VHost;
                connectionFactory.HostName = _rabbitMqsection.RabbitMqConnection.Hostname;
                return connectionFactory.CreateConnection();
        }

      private void InitializeSetup()
      {
          _channel = OpenModel();
          _brokerSetup.SetupExchangeAndQueue(_channel);
      }


      private IModel OpenModel()
        {
            if (_rabbitmqConnection == null || ! _rabbitmqConnection.IsOpen)
            {
                _rabbitmqConnection = OpenConnection();
            }
           

            IModel model = _rabbitmqConnection.CreateModel();

            _rabbitmqConnection.AutoClose = true;

            return model;
        }

    //    public void CloseModel(IModel rabbitmqChannel)
    //    {
    //        if (rabbitmqChannel != null && !rabbitmqChannel.IsClosed)
    //        {
    //            rabbitmqChannel.Close(); //TODO: use overloaded method
    //        }
    //    }
    }
}
