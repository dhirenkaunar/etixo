﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;

namespace Oxite.Data
{
    public interface IPageOfAList<T> : IList<T>
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalPageCount { get; }
        int TotalItemCount { get; }
    }
}