//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Configuration;

namespace Oxite.Configuration
{
    public class ValidationConfigurationElementCollection : ConfigurationElementCollection,
                                                            IValidationConfigurationCollection
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
            return new ValidationConfigurationElement();
        }

        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new ValidationConfigurationElement(elementName);
        }

        protected new ValidationConfigurationElement BaseGet(int index)
        {
            return (ValidationConfigurationElement)base.BaseGet(index);
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ValidationConfigurationElement)element).Name;
        }
    }
}