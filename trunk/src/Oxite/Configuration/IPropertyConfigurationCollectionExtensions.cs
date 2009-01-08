//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Specialized;

namespace Oxite.Configuration
{
    public static class IPropertyConfigurationCollectionExtensions
    {
        public static NameValueCollection AsNameValueCollection(
            this IPropertyConfigurationCollection propertyConfigurationCollection)
        {
            NameValueCollection properties = new NameValueCollection();

            if (propertyConfigurationCollection != null)
            {
                foreach (IPropertyConfiguration property in propertyConfigurationCollection)
                {
                    properties[property.Name] = property.Value;
                }
            }

            return properties;
        }
    }
}