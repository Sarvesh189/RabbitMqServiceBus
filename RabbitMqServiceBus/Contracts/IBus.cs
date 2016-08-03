using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitMqServiceBus.Contracts
{
   public interface IBus 
   {
       void PublishEvent();
       void SubscribeEvent();

       void SendCommand<T>(T baseCommand);

       void ReceiveCommand();


   }
}
