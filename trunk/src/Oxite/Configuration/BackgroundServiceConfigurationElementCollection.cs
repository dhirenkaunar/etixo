//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Configuration;

namespace Oxite.Configuration
{
    public class BackgroundServiceConfigurationElementCollection : ConfigurationElementCollection,
                                                                   IBackgroundServiceConfigurationCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        #region IBackgroundServiceConfigurationCollection Members

        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = false)]
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

        #endregion

        protected override ConfigurationElement CreateNewElement()
        {
            return new BackgroundServiceConfigurationElement();
        }

        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new BackgroundServiceConfigurationElement(elementName);
        }

        protected new BackgroundServiceConfigurationElement BaseGet(int index)
        {
            return (BackgroundServiceConfigurationElement)base.BaseGet(index);
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BackgroundServiceConfigurationElement)element).Name;
        }
    }
}