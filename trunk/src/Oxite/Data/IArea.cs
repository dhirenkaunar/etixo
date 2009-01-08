//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.Data
{
    public interface IArea
    {
        Guid SiteID { get; set; }
        Guid ID { get; set; }
        string Name { get; set; }
        string DisplayName { get; set; }
        string Description { get; set; }
        string Type { get; set; }
        string TypeUrl { get; set; }
        DateTime? Created { get; set; }
        DateTime? Modified { get; set; }
    }
}