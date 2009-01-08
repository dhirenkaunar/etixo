//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;

namespace Oxite.Data
{
    public interface IPost
    {
        IPost Parent { get; }
        Guid ID { get; set; }
        Guid CreatorUserID { get; set; }
        IUser CreatorUser { get; }
        string Title { get; set; }
        string Body { get; set; }
        string BodyShort { get; set; }
        byte State { get; set; }
        string Slug { get; set; }
        DateTime? Created { get; set; }
        DateTime? Modified { get; set; }
        DateTime? Published { get; set; }
        string SearchBody { get; }
        IArea Area { get; }
        IEnumerable<ITag> Tags { get; }
    }
}