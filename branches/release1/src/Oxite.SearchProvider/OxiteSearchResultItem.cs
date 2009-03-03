// --------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// This source code is made available under the terms of the Microsoft Public License (Ms-PL)
// http://www.codeplex.com/oxite/license
// ---------------------------------
using System;
using System.Collections.Generic;

using Oxite.Data;

namespace Oxite.Search
{
    public class OxiteSearchResultItem : ISearchResultItem
    {
        public OxiteSearchResultItem(IPost post)
        {
            //INFO: (erikpo) This shouldn't be necessary in .NET 4.0
            ID = post.ID;
            CreatorUserID = post.CreatorUserID;
            creatorUser = post.CreatorUser;
            Title = post.Title;
            Body = post.Body;
            BodyShort = post.BodyShort;
            State = post.State;
            Slug = post.Slug;
            Created = post.Created;
            Modified = post.Modified;
            Published = post.Published;
            searchBody = post.SearchBody;
            parent = post.Parent;
            Area = post.Area;
            Tags = post.Tags;
        }

        public string Permalink { get; set; }

        public Guid ID { get; set; }
        public Guid CreatorUserID { get; set; }
        private IUser creatorUser;
        public IUser CreatorUser { get { return creatorUser; } }
        public string CreatorName { get { return creatorUser.DisplayName; } }
        public string Title { get; set; }
        public string Body { get; set; }
        public string BodyShort { get; set; }
        public byte State { get; set; }
        public string Slug { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Published { get; set; }
        private string searchBody;
        public string SearchBody { get { return searchBody; } }
        private IPost parent;
        public IPost Parent { get { return parent; } }
        public IArea Area { get; private set; }
        public IEnumerable<ITag> Tags { get; private set; }
    }
}
