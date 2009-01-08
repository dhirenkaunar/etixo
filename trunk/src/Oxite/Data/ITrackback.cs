//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.Data
{
    public interface ITrackback
    {
        IPost Post { get; set; }
        Guid ID { get; set; }
        string Url { get; set; }
        string Title { get; set; }
        string Body { get; set; }
        string BlogName { get; set; }
        string Source { get; set; }
        bool? IsTargetInSource { get; set; }
        DateTime? Created { get; set; }
        DateTime? Modified { get; set; }
    }
}