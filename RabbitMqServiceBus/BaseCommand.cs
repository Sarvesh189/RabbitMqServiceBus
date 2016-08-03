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
        public Guid CommandId { get; set; }

        public Guid ApplicationId { get; set; }

        public Guid BusId { get; set; }

        public string Message { get; set; }

    }


   
}
