//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;

namespace Oxite.Data
{
    public interface IAreaRepository
    {
        IEnumerable<IArea> GetAreas(Guid siteID);
        int GetAreasCount(Guid siteID);
        IEnumerable<KeyValuePair<DateTime, int>> GetArchiveList(IArea area);
        IArea GetArea(Guid id);
        IArea GetArea(string name);
        IEnumerable<IArea> GetAllowedAreas(Guid siteID, Guid userID);
        IEnumerable<IArea> FindAreas(Guid siteID, string areaNameSearch);
        string CleanAreaName(string name);
        IArea CreateArea();
        void AddArea(IArea area);
        void AddPostToArea(Guid postID, Guid areaID);
        void RemovePostFromArea(Guid postID, Guid areaID);
        void SubmitChanges();
    }
}