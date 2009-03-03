//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Configuration;

namespace Oxite.Configuration
{
    public class SiteAliasConfigurationElement : ConfigurationElement, ISiteAliasConfiguration
    {
        public SiteAliasConfigurationElement() : base()
        {
        }

        public SiteAliasConfigurationElement(string elementName)
            : base()
        {
            Host = elementName;
        }

        #region ISiteAliasConfiguration Members

        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get
            {
                return (string)this["host"];
            }
            set
            {
                this["host"] = value;
            }
        }

        [ConfigurationProperty("redirect", IsRequired = false, DefaultValue = false)]
        public bool Redirect
        {
            get
            {
                return (bool)this["redirect"];
            }
            set
            {
                this["redirect"] = value;
            }
        }

        #endregion
    }
}