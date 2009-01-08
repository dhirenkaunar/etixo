//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oxite.Data
{
    public class OxiteBackgroundServiceActionRepository : IBackgroundServiceActionRepository
    {
        private OxiteDataContext dataContext;

        internal OxiteBackgroundServiceActionRepository(OxiteDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        #region IBackgroundServiceActionRepository Members

        public IEnumerable<IBackgroundServiceAction> GetActionsInProgress(Guid typeID)
        {
            return GetActions(typeID, true, 0);
        }

        public IEnumerable<IBackgroundServiceAction> GetActionsNotInProgress(Guid typeID, int count)
        {
            return GetActions(typeID, false, count);
        }

        public Dictionary<IPost, IBackgroundServiceAction> GetRecentlyPublishedPostsAndBackgroundServiceActions(
            Guid typeID, TimeSpan checkForNewInterval)
        {
            DateTime endDate = DateTime.Now.ToUniversalTime();
            DateTime startDate = endDate.Add(checkForNewInterval);
            var pbsa = dataContext.oxite_GetRecentlyPublishedPostsAndBackgroundServiceActions(typeID, startDate, endDate);
            Dictionary<IPost, IBackgroundServiceAction> actions = new Dictionary<IPost, IBackgroundServiceAction>(5);
            IPost p;
            IBackgroundServiceAction bsa;

            foreach (var item in pbsa)
            {
                p = (dataContext.oxite_Posts.Where(posts => posts.PostID == item.PostID)).FirstOrDefault();

                bsa = item.BackgroundServiceActionID.HasValue ? (from a in dataContext.oxite_BackgroundServiceActions
                                                                 where a.BackgroundServiceActionID == item.BackgroundServiceActionID.Value
                                                                 select a).Single() : null;

                actions.Add(p, bsa);
            }

            return actions;
        }

        public IBackgroundServiceAction CreateBackgroundServiceAction()
        {
            return new oxite_BackgroundServiceAction();
        }

        public void AddBackgroundServiceAction(IBackgroundServiceAction backgroundServiceAction)
        {
            dataContext.oxite_BackgroundServiceActions.InsertOnSubmit(
                (oxite_BackgroundServiceAction)backgroundServiceAction);
        }

        public void SubmitChanges()
        {
            dataContext.SubmitChanges();
        }

        #endregion

        public IEnumerable<IBackgroundServiceAction> GetActions(Guid typeID, bool? inProgress, int count)
        {
            IQueryable<oxite_BackgroundServiceAction> query = dataContext.oxite_BackgroundServiceActions;

            if (!inProgress.HasValue)
            {
                query = query.Where(bsa => bsa.TypeID == typeID);
            }
            else if (inProgress.Value)
            {
                query = query.Where(bsa => bsa.TypeID == typeID && bsa.InProgress == true);
            }
            else
            {
                query = query.Where(bsa => bsa.TypeID == typeID && bsa.InProgress == false && !bsa.Started.HasValue);
            }

            if (count > 0)
            {
                query = query.Take(count);
            }

            return query.Cast<IBackgroundServiceAction>();
        }
    }
}