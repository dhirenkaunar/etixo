//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Oxite.Data;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakePost : IPost
    {
        private IUser creatorUser;
        private Guid creatorUserID;

        public FakePost()
        {
            Tags = Enumerable.Empty<ITag>();
        }

        #region IPost Members

        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string BodyShort { get; set; }
        public byte State { get; set; }
        public string Slug { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Published { get; set; }
        public IArea Area { get; set; }
        public IEnumerable<ITag> Tags { get; set; }

        public IPost Parent
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Guid CreatorUserID
        {
            get
            {
                return creatorUserID;
            }
            set
            {
                creatorUserID = value;

                if (creatorUserID == Guid.Empty)
                {
                    creatorUser = null;
                }
                else
                {
                    creatorUser = new FakeUser() {ID = value};
                }
            }
        }

        public IUser CreatorUser
        {
            get
            {
                return creatorUser;
            }
            set
            {
                creatorUser = value;

                if (creatorUser == null)
                {
                    CreatorUserID = Guid.Empty;
                }
                else
                {
                    CreatorUserID = creatorUser.ID;
                }
            }
        }

        public string SearchBody
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}