using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqServiceBus.Configuration
{
    internal class RabbitMqBusEndpointElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("exchange", IsRequired = true)]
        public string Exchange
        {
            get { return (string)this["exchange"]; }
            set { this["exchange"] = value; }
        }

        [ConfigurationProperty("exchangetype", IsRequired = true)]
        public string ExchangeType
        {
            get { return (string)this["exchangetype"]; }
            set { this["exchangetype"] = value; }
        }

        [ConfigurationProperty("queue", IsRequired = true)]
        public string Queue
        {
            get { return (string)this["queue"]; }
            set { this["queue"] = value; }
        }

        [ConfigurationProperty("TTL", IsRequired = true)]
        public string TTL
        {
            get { return (string)this["TTL"]; }
            set { this["TTL"] = value; }
        }

        [ConfigurationProperty("durable", IsRequired = false)]
        public string Durable
        {
            get { return (string)this["durable"]; }
            set { this["durable"] = value; }
        }

        [ConfigurationProperty("routingkey", IsRequired = false)]
        public string Routingkey
        {
            get { return (string)this["routingkey"]; }
            set { this["routingkey"] = value; }
        }
        [ConfigurationProperty("dead-letter-routing", IsRequired = false)]
        public bool DeadletterRouting
        {
            get { return (bool)this["dead-letter-routing"]; }
            set { this["dead-letter-routing"] = value; }
        }
    }
}
