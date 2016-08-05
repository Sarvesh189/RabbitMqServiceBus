using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RabbitMqServiceBus
{
    public abstract class BaseCommand
    {
        protected BaseCommand(Guid applicationId, Guid commandId, Guid busId)
        {
            if(applicationId==Guid.Empty)
                throw new ArgumentNullException(nameof(applicationId));
            ApplicationId = applicationId;
            PublishDateTime = DateTime.UtcNow;
            if(commandId == Guid.Empty)
                throw new ArgumentNullException(nameof(commandId));
            CommandId = commandId;
            if(busId == Guid.Empty)
                throw new ArgumentNullException(nameof(busId));
            BusId = busId;
        }

        public Guid CommandId { get; set; }

        public Guid ApplicationId { get; set; }

        public Guid BusId { get; set; }

        public string Message { get; set; }

        public DateTime PublishDateTime { get; set; }

    }


   
}
