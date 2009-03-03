//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
namespace Oxite.Configuration
{
    public interface IPropertyConfiguration
    {
        string Name { get; set; }
        string Value { get; set; }
    }
}