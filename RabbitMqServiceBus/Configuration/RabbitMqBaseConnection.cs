using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqServiceBus.Configuration
{
  internal  class RabbitMqBaseConnection: ConfigurationElement
    {
        [ConfigurationProperty("uri", IsRequired = true)]
        public string Uri
        {
            get { return (string)this["uri"]; }
            set { this["uri"] = value; }
        }

        [ConfigurationProperty("port", IsRequired = true)]
        public string Port
        {
            get { return (string)this["port"]; }
            set { this["port"] = value; }
        }
        [ConfigurationProperty("connectionTimeout", IsRequired = true)]
        public string ConnectionTimeout
        {
            get { return (string)this["connectionTimeout"]; }
            set { this["connectionTimeout"] = value; }
        }

        [ConfigurationProperty("userName", IsRequired = true)]
        public string UserName
        {
            get { return (string)this["userName"]; }
            set { this["userName"] = value; }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }

        [ConfigurationProperty("hostname", IsRequired = true)]
        public string Hostname
        {
            get { return (string)this["hostname"]; }
            set { this["hostname"] = value; }
        }
        [ConfigurationProperty("vhost", IsRequired = true)]
        public string VHost
        {
            get { return (string)this["vhost"]; }
            set { this["vhost"] = value; }
        }
    }
}
