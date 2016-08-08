using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqServiceBus.Configuration
{
  internal  class RabbitMqDeadLetterExchange:ConfigurationElement
    {
        [ConfigurationProperty("exchange", IsRequired = true)]
        public string Exchange
        {
            get { return (string)this["exchange"]; }
            set { this["exchange"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string ExchangeType
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }
    }

   


}
