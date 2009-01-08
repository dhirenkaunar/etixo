//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Specialized;

namespace Oxite.Configuration
{
    public interface IBackgroundServiceConfiguration
    {
        string Name { get; set; }
        bool Enabled { get; set; }
        string Type { get; set; }
        int Interval { get; set; }
        int Timeout { get; set; }
        NameValueCollection ConfigurationProperties { get; }
    }
}