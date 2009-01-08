using System;
using System.Configuration;

namespace Oxite.Configuration
{
    public class SiteRedirectConfigurationElement : ConfigurationElement, ISiteRedirect
    {
        public SiteRedirectConfigurationElement() : base() { }

        public SiteRedirectConfigurationElement(string elementName)
            : base()
        {
            Name = elementName;
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get { return (string)this["host"]; }
            set { this["host"] = value; }
        }
    }
}
