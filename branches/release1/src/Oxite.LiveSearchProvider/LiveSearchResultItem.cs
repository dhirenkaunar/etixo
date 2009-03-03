//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Generic;
using Oxite.Data;

namespace Oxite.Search
{
    public class LiveSearchResultItem : ISearchResultItem
    {
        public string DisplayPermalink { get; set; }

        #region ISearchResultItem Members

        public string Title { get; set; }
        public string Permalink { get; set; }
        public string BodyShort { get; set; }

        public Guid ID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Guid CreatorUserID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IUser CreatorUser
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Body
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public byte State
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Slug
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime? Created
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime? Modified
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime? Published
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string SearchBody
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IPost Parent
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IArea Area
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<ITag> Tags
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string CreatorName
        {
            get
            {
                return "";
            }
        }

        #endregion
    }
}