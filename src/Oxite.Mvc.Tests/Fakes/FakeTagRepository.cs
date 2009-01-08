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
    public class FakeTagRepository : ITagRepository
    {
        public List<FakeTag> Tags;

        public FakeTagRepository()
        {
            AddedTags = new List<FakeTag>();
            Tags = new List<FakeTag>();
            Saved = false;
        }

        public FakeTagRepository(params FakeTag[] tags)
            : this()
        {
            Tags.AddRange(tags);
        }

        public List<FakeTag> AddedTags { get; set; }
        public bool Saved { get; set; }

        #region ITagRepository Members

        public IEnumerable<ITag> GetTags()
        {
            return Tags.Cast<ITag>();
        }

        public IEnumerable<KeyValuePair<ITag, int>> GetTagsWithPostCount()
        {
            throw new NotImplementedException();
        }

        public ITag GetTag(Guid id)
        {
            return Tags.Where(t => t.ID == id).FirstOrDefault();
        }

        public ITag GetTag(string name)
        {
            return Tags.Where(t => string.Compare(t.Name, name, true) == 0).FirstOrDefault();
        }

        public ITag CreateTag()
        {
            return new FakeTag();
        }

        public ITag CreateTag(string name)
        {
            return new FakeTag() {Name = name};
        }

        public void AddTag(ITag tag)
        {
            if (tag.ID == Guid.Empty)
            {
                tag.ID = Guid.NewGuid();
            }
            AddedTags.Add((FakeTag)tag);
        }

        public void SubmitChanges()
        {
            Saved = true;
        }

        #endregion
    }
}