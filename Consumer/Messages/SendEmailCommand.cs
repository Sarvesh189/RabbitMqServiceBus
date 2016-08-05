using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMqServiceBus;

namespace Consumer.Messages
{

    [CommandMetaData("Email-End-Point")]
    public class SendEmailCommand : BaseCommand
    {
        public SendEmailCommand() : base(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
        {

        }


    }
}
