using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consumer.Messages;
using RabbitMqServiceBus;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var handler = new EmailCommandHandler("Email-End-Point");
            handler.Handle<SendEmailCommand>();


            Console.ReadKey();
        }
    }

    

   
}
