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
    public class OxiteAreaRepository : IAreaRepository
    {
        private OxiteDataContext dataContext;

        internal OxiteAreaRepository(OxiteDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        #region IAreaRepository Members

        public IEnumerable<IArea> GetAreas(Guid siteID)
        {
            return getAreas(siteID).Cast<IArea>();
        }

        public int GetAreasCount(Guid siteID)
        {
            return getAreas(siteID).Count();
        }

        public IArea GetArea(Guid id)
        {
            return (from a in dataContext.oxite_Areas
                    where a.AreaID == id
                    select a).FirstOrDefault();
        }

        public IArea GetArea(string name)
        {
            return (from a in dataContext.oxite_Areas
                    where string.Compare(a.AreaName, name, true) == 0
                    select a).FirstOrDefault();
        }

        public IEnumerable<IArea> GetAllowedAreas(Guid siteID, Guid userID)
        {
            return (from urr in dataContext.oxite_UserRoleRelationships
                    join arr in dataContext.oxite_AreaRoleRelationships on urr.RoleID equals arr.RoleID
                    join a in dataContext.oxite_Areas on arr.AreaID equals a.AreaID
                    where a.SiteID == siteID && urr.UserID == userID
                    select a).Cast<IArea>();
        }

        public IEnumerable<KeyValuePair<DateTime, int>> GetArchiveList(IArea area)
        {
            var query = from p in dataContext.oxite_Posts
                        join par in dataContext.oxite_PostAreaRelationships on p.PostID equals par.PostID
                        where par.AreaID == area.ID
                        select p;

            return query.Visible().ArchiveList();
        }

        public IEnumerable<IArea> FindAreas(Guid siteID, string areaNameSearch)
        {
            return (from a in dataContext.oxite_Areas
                    where a.SiteID == siteID && a.AreaName.Contains(areaNameSearch)
                    select a).Cast<IArea>();
        }

        public string CleanAreaName(string name)
        {
            Regex exp = new Regex(@"[^A-Za-z0-9]", RegexOptions.Compiled);
            return exp.Replace(name, "");
        }

        public IArea CreateArea()
        {
            return new oxite_Area();
        }

        public void AddArea(IArea area)
        {
            if (area.SiteID == Guid.Empty)
            {
                throw new ArgumentException("SiteID must be set", "area.SiteID");
            }
            if (string.IsNullOrEmpty(area.Name))
            {
                throw new ArgumentNullException("area.Name");
            }
            area.Name = CleanAreaName(area.Name);
            if (string.IsNullOrEmpty(area.Name))
            {
                throw new ArgumentException("area.Name does not contain any valid characters", "area.Name");
            }
            if (area.ID == Guid.Empty)
            {
                area.ID = Guid.NewGuid();
            }
            if (area.DisplayName == null)
            {
                area.DisplayName = "";
            }
            if (!area.Created.HasValue)
            {
                area.Created = DateTime.Now.ToUniversalTime();
            }
            if (!area.Modified.HasValue)
            {
                area.Modified = area.Created;
            }

            dataContext.oxite_Areas.InsertOnSubmit((oxite_Area)area);
        }

        public void AddPostToArea(Guid postID, Guid areaID)
        {
            dataContext.oxite_PostAreaRelationships.InsertOnSubmit(new oxite_PostAreaRelationship
                                                                   {PostID = postID, AreaID = areaID});
        }

        public void RemovePostFromArea(Guid postID, Guid areaID)
        {
            oxite_PostAreaRelationship pbrInstance =
                dataContext.oxite_PostAreaRelationships.Where(par => par.PostID == postID && par.AreaID == areaID).
                    FirstOrDefault();

            if (pbrInstance != null)
            {
                dataContext.oxite_PostAreaRelationships.DeleteOnSubmit(pbrInstance);
            }
        }

        public void SubmitChanges()
        {
            dataContext.SubmitChanges();
        }

        #endregion

        private IQueryable<oxite_Area> getAreas(Guid siteID)
        {
            return from a in dataContext.oxite_Areas
                   where a.SiteID == siteID
                   orderby a.AreaName
                   select a;
        }
    }
}