using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMqServiceBus.Configuration;
using RabbitMqServiceBus.Utility;
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
            IConnection connection=null;
            var connectionFactory = new ConnectionFactory();
            connectionFactory = SetUpConnectionProperty(connectionFactory, _rabbitMqsection.RabbitMqConnection);
            try
            {
                connection = connectionFactory.CreateConnection();
            }
            catch (Exception firstEx)
            {
                Logger.WriteError(firstEx.ToString());
                try
                {
                    connectionFactory = SetUpConnectionProperty(connectionFactory,
                        _rabbitMqsection.RabbitMqAlternateConnection);
                    connection = connectionFactory.CreateConnection();
                }
                catch (Exception secEx)
                {
                    Logger.WriteError(secEx.ToString());
                    throw;
                }
            }


            return connection;
        }

      private ConnectionFactory SetUpConnectionProperty(ConnectionFactory connectionFactory, RabbitMqBaseConnection rabbitMqConnection)
      {
            connectionFactory.AutomaticRecoveryEnabled = true;
            connectionFactory.HandshakeContinuationTimeout = TimeSpan.FromMinutes(int.Parse(rabbitMqConnection.ConnectionTimeout));
            connectionFactory.ContinuationTimeout = TimeSpan.FromMinutes(int.Parse(rabbitMqConnection.ConnectionTimeout));
            connectionFactory.UserName = rabbitMqConnection.UserName;
            connectionFactory.Password = rabbitMqConnection.Password;
            connectionFactory.Port = int.Parse(rabbitMqConnection.Port);
            connectionFactory.Protocol = _protocol;
            connectionFactory.VirtualHost = rabbitMqConnection.VHost;
            connectionFactory.HostName = rabbitMqConnection.Hostname;
          return connectionFactory;
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
