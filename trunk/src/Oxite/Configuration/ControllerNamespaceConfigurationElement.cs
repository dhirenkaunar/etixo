//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Configuration;

namespace Oxite.Configuration
{
    public class ControllerNamespaceConfigurationElement : ConfigurationElement, IControllerNamespaceConfiguration
    {
        public ControllerNamespaceConfigurationElement() : base()
        {
        }

        public ControllerNamespaceConfigurationElement(string elementName)
            : base()
        {
            Namespace = elementName;
        }

        #region IControllerNamespaceConfiguration Members

        [ConfigurationProperty("namespace", IsRequired = true, IsKey = true)]
        public string Namespace
        {
            get
            {
                return (string)this["namespace"];
            }
            set
            {
                this["namespace"] = value;
            }
        }

        #endregion
    }
}