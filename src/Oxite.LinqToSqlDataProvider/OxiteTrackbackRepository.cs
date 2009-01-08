//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Linq;

namespace Oxite.Data
{
    public class OxiteTrackbackRepository : ITrackbackRepository
    {
        private OxiteDataContext dataContext;

        internal OxiteTrackbackRepository(OxiteDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        #region ITrackbackRepository Members

        public ITrackback GetTrackback(IPost post, string url)
        {
            return (from tb in dataContext.oxite_Trackbacks
                    where tb.PostID == post.ID && string.Compare(tb.Url, url, true) == 0
                    select tb).FirstOrDefault();
        }

        public ITrackback CreateTrackback()
        {
            return new oxite_Trackback();
        }

        public void AddTrackback(ITrackback trackback)
        {
            dataContext.oxite_Trackbacks.InsertOnSubmit((oxite_Trackback)trackback);
        }

        public void SubmitChanges()
        {
            dataContext.SubmitChanges();
        }

        #endregion
    }
}