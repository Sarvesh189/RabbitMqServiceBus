using System;
using RabbitMqBusClient.Messages;
using RabbitMqServiceBus;


namespace RabbitMqBusClient
{
    class Program
    {
      
        static void Main(string[] args)
        {

               Guid applicationId = Guid.NewGuid(); //one for an application
              Guid busId = Guid.NewGuid(); //Each instance of publishing bus
            var msg = Console.ReadLine();
            while (msg != "Q")
            {
                var commandMessage = msg;
                var rmBus = new RabbitMqBus();
                rmBus.SendCommand(new SendEmailCommand()
                {
                   
                    Message = commandMessage
                });
              

                Console.WriteLine("published");
                 msg = Console.ReadLine();
            }
            Console.ReadKey();
        }
    }

    

    
   
}
