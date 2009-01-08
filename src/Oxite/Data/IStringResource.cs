//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.Data
{
    public interface IStringResource
    {
        string Key { get; set; }
        string Value { get; set; }
        string Language { get; set; }
        short Version { get; set; }
        Guid CreatorUserID { get; set; }
        DateTime? Created { get; set; }
    }
}