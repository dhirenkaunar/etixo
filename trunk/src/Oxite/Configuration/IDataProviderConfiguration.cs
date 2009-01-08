//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
namespace Oxite.Configuration
{
    public interface IDataProviderConfiguration
    {
        string Name { get; set; }
        string Type { get; set; }
        string ConnectionStringName { get; set; }
    }
}