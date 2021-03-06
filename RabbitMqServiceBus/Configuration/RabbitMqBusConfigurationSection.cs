﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace RabbitMqServiceBus.Configuration
{
    internal class RabbitMqBusEndpointsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RabbitMqBusEndpointElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RabbitMqBusEndpointElement)element).Name;
        }
    }



    internal class RabbitMqBusConfigurationSection: ConfigurationSection
    {
        public const string SectionName = "RabbitMqBusConfigurationSection";

        private const string EndpointCollectionName = "RabbitMqBusEndPoints";

        [ConfigurationProperty("rabbitMqConnection")]
        public RabbitMqConnection RabbitMqConnection
        {
            get
            {
                return (RabbitMqConnection)this["rabbitMqConnection"];
            }
            set
            { this["rabbitMqConnection"] = value; }
        }

        [ConfigurationProperty("rabbitMqAlternateConnection",IsRequired = false)]
        public RabbitMqAlternateConnection RabbitMqAlternateConnection
        {
            get
            {
                return (RabbitMqAlternateConnection)this["rabbitMqAlternateConnection"];
            }
            set
            { this["rabbitMqAlternateConnection"] = value; }
        }
        [ConfigurationProperty("deadletterexchange")]
        public RabbitMqDeadLetterExchange DeadLetterExchange
        {
            get { return (RabbitMqDeadLetterExchange) this["deadletterexchange"]; }
            set { this["deadletterexchange"] = value; }
        }


        [ConfigurationProperty(EndpointCollectionName)]
        [ConfigurationCollection(typeof(RabbitMqBusEndpointsCollection), AddItemName = "add")]
        public RabbitMqBusEndpointsCollection ConnectionManagerEndpoints
        {
            get
            {
                return (RabbitMqBusEndpointsCollection)base[EndpointCollectionName]; 
                
            }
        }
       
    }


}
