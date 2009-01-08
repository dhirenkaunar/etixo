﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.Data
{
    public interface ITag
    {
        ITag Parent { get; }
        Guid ID { get; set; }
        string Name { get; set; }
        DateTime? Created { get; set; }
    }
}