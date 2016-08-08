using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMqServiceBus.Contracts;

namespace RabbitMqServiceBus
{
   public class RabbitMqBus:IBus
   {
       public void PublishEvent()
       {
           throw new NotImplementedException();
       }

       public void SubscribeEvent()
       {
           throw new NotImplementedException();
       }

      

       public void ReceiveCommand()
       {
           throw new NotImplementedException();
       }

        public void SendCommand<T>(T command)
        {
            var publisher = new BasePublisher();
            publisher.Publish<T>(command);
        }
    }
}
