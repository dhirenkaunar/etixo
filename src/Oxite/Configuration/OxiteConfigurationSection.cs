//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Configuration;

namespace Oxite.Configuration
{
    public class OxiteConfigurationSection : ConfigurationSection, IOxiteConfiguration
    {
        private string[] controllerNamespaces;

        [ConfigurationProperty("site")]
        public SiteConfigurationElement SiteInternal
        {
            get
            {
                return (SiteConfigurationElement)this["site"];
            }
            set
            {
                this["site"] = value;
            }
        }

        [ConfigurationProperty("controllerNamespaces")]
        public ControllerNamespacesConfigurationElementCollection ControllerNamespacesInternal
        {
            get
            {
                return (ControllerNamespacesConfigurationElementCollection)this["controllerNamespaces"];
            }
            set
            {
                this["controllerNamespaces"] = value;
            }
        }

        [ConfigurationProperty("routes")]
        public RoutingRegistrationConfigurationElement RoutesInternal
        {
            get
            {
                return (RoutingRegistrationConfigurationElement)this["routes"];
            }
            set
            {
                this["routes"] = value;
            }
        }

        [ConfigurationProperty("dataProvider")]
        public DataProviderConfigurationElement DataProviderInternal
        {
            get
            {
                return (DataProviderConfigurationElement)this["dataProvider"];
            }
            set
            {
                this["dataProvider"] = value;
            }
        }

        [ConfigurationProperty("searchProvider")]
        public SearchProviderConfigurationElement SearchProviderInternal
        {
            get
            {
                return (SearchProviderConfigurationElement)this["searchProvider"];
            }
            set
            {
                this["searchProvider"] = value;
            }
        }

        [ConfigurationProperty("backgroundServices")]
        public BackgroundServiceConfigurationElementCollection BackgroundServicesInternal
        {
            get
            {
                return (BackgroundServiceConfigurationElementCollection)this["backgroundServices"];
            }
            set
            {
                this["backgroundServices"] = value;
            }
        }

        [ConfigurationProperty("validation")]
        public ValidationConfigurationElementCollection ValidationInternal
        {
            get
            {
                return (ValidationConfigurationElementCollection)this["validation"];
            }
            set
            {
                this["validation"] = value;
            }
        }

        #region IOxiteConfiguration Members

        public ISiteConfiguration Site
        {
            get
            {
                return SiteInternal;
            }
        }

        public string[] ControllerNamespaces
        {
            get
            {
                if (controllerNamespaces == null)
                {
                    if (ControllerNamespacesInternal != null && ControllerNamespacesInternal.Count > 0)
                    {
                        controllerNamespaces = new string[ControllerNamespacesInternal.Count];
                        int counter = 0;

                        foreach (IControllerNamespaceConfiguration controllerNamespace in ControllerNamespacesInternal)
                        {
                            controllerNamespaces[counter] = controllerNamespace.Namespace;

                            counter++;
                        }
                    }
                }

                return controllerNamespaces;
            }
        }

        public IRoutingRegistrationConfiguration Routes
        {
            get
            {
                return RoutesInternal;
            }
        }

        public IDataProviderConfiguration DataProvider
        {
            get
            {
                return DataProviderInternal;
            }
        }

        public ISearchProviderConfiguration SearchProvider
        {
            get
            {
                return SearchProviderInternal;
            }
        }

        public IBackgroundServiceConfigurationCollection BackgroundServices
        {
            get
            {
                return BackgroundServicesInternal;
            }
        }

        public IValidationConfigurationCollection Validation
        {
            get
            {
                return ValidationInternal;
            }
        }

        #endregion
    }
}