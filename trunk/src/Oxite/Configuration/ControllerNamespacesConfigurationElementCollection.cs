//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Configuration;

namespace Oxite.Configuration
{
    public class ControllerNamespacesConfigurationElementCollection : ConfigurationElementCollection,
                                                                      IControllerNamespacesConfigurationCollection
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
            return new ControllerNamespaceConfigurationElement();
        }

        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new ControllerNamespaceConfigurationElement(elementName);
        }

        protected new ControllerNamespaceConfigurationElement BaseGet(int index)
        {
            return (ControllerNamespaceConfigurationElement)base.BaseGet(index);
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ControllerNamespaceConfigurationElement)element).Namespace;
        }
    }
}