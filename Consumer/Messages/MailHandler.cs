using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMqServiceBus;

namespace Consumer.Messages
{
    public class EmailCommandHandler : BaseCommandHandler
    {
        public EmailCommandHandler(string key) : base(key)
        {
        }

        public override void ProcessMessage<T>(T command)
        {
            Console.WriteLine(command);
            SendEmail(command as SendEmailCommand);
        }

        private void SendEmail(SendEmailCommand objCommand)
        {
            Console.WriteLine(objCommand.CommandId);
           Console.WriteLine(objCommand.Message);
        }

    }
}
