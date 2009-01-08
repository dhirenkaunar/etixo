//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Specialized;
using System.Configuration;

namespace Oxite.Configuration
{
    public class SearchProviderConfigurationElement : ConfigurationElement, ISearchProviderConfiguration
    {
        private NameValueCollection configurationProperties;

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

        #region ISearchProviderConfiguration Members

        [ConfigurationProperty("providerName", IsRequired = true)]
        public string ProviderName
        {
            get
            {
                return (string)this["providerName"];
            }
            set
            {
                this["providerName"] = value;
            }
        }

        [ConfigurationProperty("providerType", IsRequired = true)]
        public string ProviderType
        {
            get
            {
                return (string)this["providerType"];
            }
            set
            {
                this["providerType"] = value;
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