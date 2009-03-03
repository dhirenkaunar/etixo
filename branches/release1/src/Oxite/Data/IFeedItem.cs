//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;

namespace Oxite.Data
{
    public interface IFeedItem
    {
        string Title { get; }
        string Body { get; }
        DateTime? Published { get; }
        string CreatorName { get; }
        IArea Area { get; }
        IEnumerable<ITag> Tags { get; }
    }
}