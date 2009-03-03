//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;

namespace Oxite.Data
{
    public interface IBackgroundServiceActionRepository
    {
        IEnumerable<IBackgroundServiceAction> GetActionsInProgress(Guid typeID);
        IEnumerable<IBackgroundServiceAction> GetActionsNotInProgress(Guid typeID, int count);

        Dictionary<IPost, IBackgroundServiceAction> GetRecentlyPublishedPostsAndBackgroundServiceActions(Guid typeID,
                                                                                                         TimeSpan
                                                                                                             checkForNewInterval);

        IBackgroundServiceAction CreateBackgroundServiceAction();
        void AddBackgroundServiceAction(IBackgroundServiceAction backgroundServiceAction);
        void SubmitChanges();
    }
}