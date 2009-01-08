//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Configuration;

namespace Oxite.Configuration
{
    public class SiteAliasConfigurationElementCollection : ConfigurationElementCollection,
                                                           ISiteAliasConfigurationCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SiteAliasConfigurationElement();
        }

        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new SiteAliasConfigurationElement(elementName);
        }

        protected new SiteAliasConfigurationElement BaseGet(int index)
        {
            return (SiteAliasConfigurationElement)base.BaseGet(index);
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SiteAliasConfigurationElement)element).Host;
        }
    }
}