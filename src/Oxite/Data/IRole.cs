﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.Data
{
    public interface IRole
    {
        IRole Parent { get; set; }
        Guid ID { get; set; }
        string Name { get; set; }
    }
}