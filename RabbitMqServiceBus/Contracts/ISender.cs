using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqServiceBus.Contracts
{
   internal interface ISender
   {
     void  SendMessage();
   }
}
