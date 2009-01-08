//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using Oxite.Configuration;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeConfig : IOxiteConfiguration
    {
        public FakeConfig()
        {
        }

        public FakeConfig(FakeSite site)
            : this()
        {
            Site = site;
        }

        #region IOxiteConfiguration Members

        public ISiteConfiguration Site { get; set; }

        public string[] ControllerNamespaces
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IRoutingRegistrationConfiguration Routes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDataProviderConfiguration DataProvider
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ISearchProviderConfiguration SearchProvider
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IBackgroundServiceConfigurationCollection BackgroundServices
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IValidationConfigurationCollection Validation
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}