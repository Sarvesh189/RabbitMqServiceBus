# RabbitMqServiceBus

RabbitMqServiceBus is a servicebus application over RabbitMq message broker.
This is primary developed for work queues which distribute the task among worker applications.

##How to use this library
  Below are the listed steps to use library in application. RabbitMq Service should be installed on the box and 
  we should have the below information
  
	- vhost, 
	
	- host,
	
	- port,
	
	- username,
	
	- password
	
	Lets assume that for the testing below is the information
	
	- vhost: testvhost,
	
	- host: testhost,
	
	- port: 5672,
	
	- username: testadmin,
	
	- password: testpwd

###Command Publisher Application
Install the nuget package for the RabbitMqServiceBus with the below command

    Install-Package RabbitMqServiceBus
    
Make the below entry in app.config

**&lt;configsections&gt; entry** 

	<section name="RabbitMqBusConfigurationSection" type="RabbitMqServiceBus.Configuration.RabbitMqBusConfigurationSection,RabbitMqServiceBus"/>


**and then below entry** 
      
	  
       <RabbitMqBusConfigurationSection>
          <rabbitMqConnection uri="" vhost=testvhost" port="5672" connectionTimeout="20" userName="testadmin" password="testpwd" hostname="testhost"></rabbitMqConnection>
          <deadletterexchange exchange="test.exchange.dead" type="direct"></deadletterexchange>
          <RabbitMqBusEndPoints>
            <add name="Email-End-Point" exchange="test.email" exchangetype="direct" queue="test-email-queue" TTL="60" durable="True" routingkey="emailqueue" dead-letter-routing="True"></add>
            <add name="Email-dead-EndPoint" exchange="test.exchange.dead" exchangetype="direct" queue="test-email-dead-queue" TTL="-1" durable="True" routingkey="emailqueue" dead-letter-routing="False"></add>
          <RabbitMqBusEndPoints>
        </RabbitMqBusConfigurationSection>
	
###rabbitMqConnection

uri keep empty. Rest other entries are from the rabbitmq service information.
	
####deadletterexchange
This exchange keeps the dirty messages and expired messages.

    exchange: put meaningful name.
  	type: direct

####RabbitMqBusEndPoints 
  Add element with below information

		name: give meaningful endpoint name
		exchange:name of the exchange e.g. applicationName.email
		exchangetype: direct
		queue: queue name
		TTL: time to live. It is in minutes. This is time message will stay in queue and after this, message will be moved to deadletterexchange.
		durable: message will persist during restart of machine or service
		routingkey: meaningful name. Exchange will use routing key to route the message to relevant queue.
		dead-letter-routing: true. dirty messages will be routed to the deadletterexchange.
       
Add another endpoint for the deadletterexchange similar to above and as show in sample entry.

####Develop command class as below
	
	 [CommandMetaData("Email-End-Point")] 
    public class SendEmailCommand : BaseCommand
    {
        public SendEmailCommand(Guid applicationId, Guid commandId, Guid busId):base(applicationId, commandId, busId)
        {
        }   
    }
	
	
SendEmailCommand is an Command object. 
	
Any command object must inherit BaseCommand.
	
It needs to be decorated with the attribute __CommandMetaData__ with RabbitMq End point mentioned in the configuration.
	
Then in the main method 
	
	
				var rmBus = new RabbitMqBus();
				var commandMessage="test email message";
				var applicationId=Guid.NewGuid();
				var commandId=Guid.NewGuid();
				var busId = Guid.NewGuid();
                rmBus.SendCommand(new SendEmailCommand()
                {
                   
                    Message = commandMessage
                });
                
#### Command Handler Application
Install the nuget package for the RabbitMqServiceBus with the below command

    Install-Package RabbitMqServiceBus
	
Make the below entry in app.config
	
__<configsections> entry__

    <section name="RabbitMqBusConfigurationSection" type="RabbitMqServiceBus.Configuration.RabbitMqBusConfigurationSection,RabbitMqServiceBus"/>
	

and then below entry 
      
    <RabbitMqBusConfigurationSection>
    <rabbitMqConnection uri="" vhost=testvhost" port="5672" connectionTimeout="20" userName="testadmin" password="testpwd" hostname="testhost"></rabbitMqConnection>
    <deadletterexchange exchange="test.exchange.dead" type="direct"></deadletterexchange>
      <RabbitMqBusEndPoints>
        <add name="Email-End-Point" exchange="test.email" exchangetype="direct" queue="test-email-queue" TTL="60" durable="True" routingkey="emailqueue" dead-letter-routing="True"></add>
        <add name="Email-dead-EndPoint" exchange="test.exchange.dead" exchangetype="direct" queue="test-email-dead-queue" TTL="-1" durable="True" routingkey="emailqueue" dead-letter-routing="False"></add>
      <RabbitMqBusEndPoints>
    </RabbitMqBusConfigurationSection>
	
Create Commnd Object
	
	  [CommandMetaData("Email-End-Point")]
    public class SendEmailCommand : BaseCommand
    {
        public SendEmailCommand() : base(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid())
        {
        }
    }
	
Develop Handler object
	

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

	
In the main method
	
	   var handler = new EmailCommandHandler("Email-End-Point");
            handler.Handle<SendEmailCommand>();

	
