using System;
using RabbitMqServiceBus;


namespace RabbitMqBusClient
{
    class Program
    {
        static void Main(string[] args)
        {
           var rmBus = new RabbitMqBus();
            rmBus.SendCommand(new SendEmail2Command() { CommandId = Guid.NewGuid()});

      //      var handler = new EmailCommandHandler("Email-End-Point");
      //      handler.Handle<SendEmail2Command>();


            Console.ReadKey();
        }
    }

    [CommandMetaData("Email-End-Point")]
    public class SendEmail2Command : BaseCommand
    {
        public SendEmail2Command()
        {

        }


    }

    [CommandMetaData("Email-End-Point")]
    public class SendEmailCommand : BaseCommand
    {
        public SendEmailCommand()
        {

        }


    }
    public class EmailCommandHandler : BaseCommandHandler
    {
        public EmailCommandHandler(string key):base(key)
        {
        }

        public override void ProcessMessage<T>(T command)
        {
          Console.WriteLine(command);
          SendEmail(command as SendEmail2Command);
        }

        private void SendEmail(SendEmail2Command objCommand)
        {
            Console.WriteLine(objCommand.CommandId);
            throw new Exception("issue in processing");
        }

    }

   
}
