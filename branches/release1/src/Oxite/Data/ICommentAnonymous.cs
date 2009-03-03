//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Runtime.Serialization;

namespace Oxite.Data
{
    public interface ICommentAnonymous
    {
        Guid CommentID { get; set; }
        IComment Comment { get; }
        string Email { get; set; }

        [DataMember]
        string HashedEmail { get; set; }

        [DataMember]
        string Name { get; set; }

        [DataMember]
        string Url { get; set; }
    }
}