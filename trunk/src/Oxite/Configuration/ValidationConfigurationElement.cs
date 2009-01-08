//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Configuration;

namespace Oxite.Configuration
{
    public class ValidationConfigurationElement : ConfigurationElement, IValidationConfiguration
    {
        public ValidationConfigurationElement() : base()
        {
        }

        public ValidationConfigurationElement(string elementName)
            : base()
        {
            Name = elementName;
        }

        #region IValidationConfiguration Members

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

        [ConfigurationProperty("regex", IsRequired = true)]
        public string Regex
        {
            get
            {
                return (string)this["regex"];
            }
            set
            {
                this["regex"] = value;
            }
        }

        [ConfigurationProperty("regexOptions", IsRequired = false, DefaultValue = 0)]
        public int RegexOptions
        {
            get
            {
                return (int)this["regexOptions"];
            }
            set
            {
                this["regexOptions"] = value;
            }
        }

        #endregion
    }
}