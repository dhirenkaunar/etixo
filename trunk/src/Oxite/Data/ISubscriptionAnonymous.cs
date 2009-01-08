//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.Data
{
    public interface ISubscriptionAnonymous
    {
        Guid SubscriptionID { get; set; }
        ISubscription Subscription { get; }
        string Name { get; set; }
        string Email { get; set; }
    }
}