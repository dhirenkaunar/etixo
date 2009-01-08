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
    public class FakeAreaRepository : IAreaRepository
    {
        public List<FakeArea> Areas;

        public FakeAreaRepository()
        {
            Areas = new List<FakeArea>();
            AddedAreas = new List<FakeArea>();
            AddedPostAreas = new List<KeyValuePair<Guid, Guid>>();
            Saved = false;
        }

        public FakeAreaRepository(params FakeArea[] area)
            : this()
        {
            Areas.AddRange(area);
        }

        public List<FakeArea> AddedAreas { get; set; }
        public List<KeyValuePair<Guid, Guid>> AddedPostAreas { get; set; }
        public bool Saved { get; set; }

        #region IAreaRepository Members

        public IEnumerable<IArea> GetAreas(Guid siteID)
        {
            return Areas.Cast<IArea>();
        }

        public int GetAreasCount(Guid siteID)
        {
            return Areas.Count;
        }

        public IEnumerable<KeyValuePair<DateTime, int>> GetArchiveList(IArea area)
        {
            throw new NotImplementedException();
        }

        public IArea GetArea(Guid id)
        {
            return Areas.Where(a => a.ID == id).FirstOrDefault();
        }

        public IArea GetArea(string name)
        {
            return Areas.Where(a => string.Compare(a.Name, name, true) == 0).FirstOrDefault();
        }

        public IEnumerable<IArea> GetAllowedAreas(Guid siteID, Guid userID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IArea> FindAreas(Guid siteID, string areaNameSearch)
        {
            throw new NotImplementedException();
        }

        public string CleanAreaName(string name)
        {
            throw new NotImplementedException();
        }

        public IArea CreateArea()
        {
            throw new NotImplementedException();
        }

        public void AddArea(IArea area)
        {
            AddedAreas.Add((FakeArea)area);
        }

        public void AddPostToArea(Guid postID, Guid areaID)
        {
        }

        public void RemovePostFromArea(Guid postID, Guid areaID)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            Saved = true;
        }

        #endregion
    }
}