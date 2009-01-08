//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.Data
{
    public interface ISubscription
    {
        Guid ID { get; set; }
        Guid PostID { get; set; }
        Guid UserID { get; set; }
        IPost Post { get; }
        IUser User { get; }
        ISubscriptionAnonymous SubscriptionAnonymous { get; }
    }
}