using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqServiceBus
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class CommandMetaDataAttribute:Attribute
    {
        public CommandMetaDataAttribute(string key)
        {
            this.Key = key;
        }

        public string  Key { get; set; }
    }
}
