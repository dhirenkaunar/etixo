//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Oxite.Data
{
    public class OxiteTagRepository : ITagRepository
    {
        private OxiteDataContext dataContext;

        internal OxiteTagRepository(OxiteDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        #region ITagRepository Members

        public IEnumerable<ITag> GetTags()
        {
            return dataContext.oxite_Tags.Cast<ITag>();
        }

        public IEnumerable<KeyValuePair<ITag, int>> GetTagsWithPostCount()
        {
            return from tt in
                       (from t in dataContext.oxite_Tags
                        join ptr in dataContext.oxite_PostTagRelationships on t.TagID equals ptr.TagID
                        join p in dataContext.oxite_Posts on ptr.PostID equals p.PostID
                        where p.State == (byte)EntityState.Normal && p.PublishedDate <= DateTime.Now.ToUniversalTime()
                        select new {Tag = t.oxite_Tag1, Post = p})
                   group tt by tt.Tag
                   into results
                       where results.Key.TagID == results.Key.ParentTagID
                       orderby results.Key.TagName
                       select new KeyValuePair<ITag, int>(results.Key, results.Count());
        }

        public ITag GetTag(string name)
        {
            return (from t in dataContext.oxite_Tags
                    where string.Compare(t.TagName, name, true) == 0
                    select t).FirstOrDefault();
        }

        public ITag GetTag(Guid id)
        {
            return (from t in dataContext.oxite_Tags
                    where Guid.Equals(t.ID, id)
                    select t).FirstOrDefault();
        }


        public ITag CreateTag()
        {
            return new oxite_Tag();
        }

        public ITag CreateTag(string name)
        {
            return new oxite_Tag() {Name = name};
        }

        public void AddTag(ITag tag)
        {
            if (string.IsNullOrEmpty(tag.Name))
            {
                throw new ArgumentNullException("tag.Name");
            }

            if (tag.ID == Guid.Empty)
            {
                tag.ID = Guid.NewGuid();
            }
            tag.Created = DateTime.Now.ToUniversalTime();

            Regex exp = new Regex(@"[^A-Za-z0-9]", RegexOptions.Compiled);
            string name = exp.Replace(tag.Name, "");
            ITag parent = GetTag(name);

            if (parent == null && string.Compare(tag.Name, name, true) != 0)
            {
                Guid id = Guid.NewGuid();

                parent = new oxite_Tag()
                         {ParentTagID = id, TagID = id, TagName = name, CreatedDate = DateTime.Now.ToUniversalTime()};

                dataContext.oxite_Tags.InsertOnSubmit((oxite_Tag)parent);
            }

            if (parent != null)
            {
                ((oxite_Tag)tag).ParentTagID = parent.ID;
            }
            else
            {
                ((oxite_Tag)tag).ParentTagID = tag.ID;
            }

            dataContext.oxite_Tags.InsertOnSubmit((oxite_Tag)tag);
        }

        public void SubmitChanges()
        {
            dataContext.SubmitChanges();
        }

        #endregion
    }
}