//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
namespace Oxite.Configuration
{
    public interface IOxiteConfiguration
    {
        ISiteConfiguration Site { get; }
        string[] ControllerNamespaces { get; }
        IRoutingRegistrationConfiguration Routes { get; }
        IDataProviderConfiguration DataProvider { get; }
        ISearchProviderConfiguration SearchProvider { get; }
        IBackgroundServiceConfigurationCollection BackgroundServices { get; }
        IValidationConfigurationCollection Validation { get; }
    }
}