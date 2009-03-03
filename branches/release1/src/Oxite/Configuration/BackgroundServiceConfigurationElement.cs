//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Specialized;
using System.Configuration;

namespace Oxite.Configuration
{
    public class BackgroundServiceConfigurationElement : ConfigurationElement, IBackgroundServiceConfiguration
    {
        private NameValueCollection configurationProperties;

        public BackgroundServiceConfigurationElement() : base()
        {
        }

        public BackgroundServiceConfigurationElement(string elementName)
            : base()
        {
            Name = elementName;
        }

        [ConfigurationProperty("properties")]
        public PropertyConfigurationElementCollection ConfigurationPropertiesInternal
        {
            get
            {
                return (PropertyConfigurationElementCollection)this["properties"];
            }
            set
            {
                this["Properties"] = value;
            }
        }

        #region IBackgroundServiceConfiguration Members

        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = true)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return (string)this["type"];
            }
            set
            {
                this["type"] = value;
            }
        }

        [ConfigurationProperty("interval", IsRequired = false, DefaultValue = 300)]
        public int Interval
        {
            get
            {
                return (int)this["interval"];
            }
            set
            {
                this["interval"] = value;
            }
        }

        [ConfigurationProperty("timeout", IsRequired = false, DefaultValue = 60)]
        public int Timeout
        {
            get
            {
                return (int)this["timeout"];
            }
            set
            {
                this["timeout"] = value;
            }
        }

        public NameValueCollection ConfigurationProperties
        {
            get
            {
                if (configurationProperties == null)
                {
                    configurationProperties = ConfigurationPropertiesInternal.AsNameValueCollection();
                }

                return configurationProperties;
            }
        }

        #endregion
    }
}