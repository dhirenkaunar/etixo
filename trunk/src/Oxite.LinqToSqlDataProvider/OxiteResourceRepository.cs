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
    public class OxiteResourceRepository : IResourceRepository
    {
        private OxiteDataContext dataContext;

        internal OxiteResourceRepository(OxiteDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        #region IResourceRepository Members

        public IFileResource GetFile(Guid siteID, string path, string name)
        {
            return (from f in dataContext.oxite_FileResources
                    where
                        f.SiteID == siteID && string.Compare(f.Path, path, true) == 0 &&
                        string.Compare(f.Name, name, true) == 0
                    select f).FirstOrDefault();
        }

        public IFileResource GetFile(Guid siteID, Guid userID, Guid fileResourceID)
        {
            return (from ufr in dataContext.oxite_UserFileResourceRelationships
                    join f in dataContext.oxite_FileResources on ufr.FileResourceID equals f.FileResourceID
                    where
                        ufr.UserID == userID &&
                        f.FileResourceID == fileResourceID &&
                        f.SiteID == siteID
                    select f).FirstOrDefault();
        }

        public IFileResource GetFile(Guid siteID, Guid userID, string filePath)
        {
            return (from ufr in dataContext.oxite_UserFileResourceRelationships
                    join f in dataContext.oxite_FileResources on ufr.FileResourceID equals f.FileResourceID
                    where
                        ufr.UserID == userID &&
                        string.Compare(f.FileResourceName, filePath, true) == 0 &&
                        f.SiteID == siteID &&
                        f.State == (byte)EntityState.Normal
                    select f).FirstOrDefault();
        }

        public IEnumerable<IFileResource> GetFiles(Guid siteID, Guid userID)
        {
            return (from ufr in dataContext.oxite_UserFileResourceRelationships
                    join f in dataContext.oxite_FileResources on ufr.FileResourceID equals f.FileResourceID
                    where
                        ufr.UserID == userID &&
                        f.SiteID == siteID &&
                        f.State == (byte)EntityState.Normal
                    select f).Cast<IFileResource>();
        }

        public IFileResource CreateFile()
        {
            return new oxite_FileResource();
        }

        public void AddFile(IFileResource fileResource)
        {
            if (fileResource.ID == Guid.Empty)
            {
                fileResource.ID = Guid.NewGuid();
            }
            fileResource.Created = fileResource.Modified = DateTime.Now.ToUniversalTime();

            //HACK: (erikpo) This is to get around the problem of LINQ to SQL trying to get reference tables when doing the insert and causing an exception
            ((oxite_FileResource)fileResource).PrepareForInsert();

            dataContext.oxite_FileResources.InsertOnSubmit((oxite_FileResource)fileResource);
        }

        public void AddFileToUser(Guid fileResourceID, Guid userID)
        {
            dataContext.oxite_UserFileResourceRelationships.InsertOnSubmit(new oxite_UserFileResourceRelationship()
                                                                           {
                                                                               FileResourceID = fileResourceID,
                                                                               UserID = userID
                                                                           });
        }

        public void RemoveFileFromUser(Guid fileResourceID, Guid userID)
        {
            oxite_UserFileResourceRelationship ufrrInstance =
                dataContext.oxite_UserFileResourceRelationships.Where(
                    ufrr => ufrr.FileResourceID == fileResourceID && ufrr.UserID == userID).FirstOrDefault();

            if (ufrrInstance != null)
            {
                dataContext.oxite_UserFileResourceRelationships.DeleteOnSubmit(ufrrInstance);
            }
        }

        public void RemoveFile(Guid id)
        {
            oxite_FileResource fileResource = (from fr in dataContext.oxite_FileResources
                                               where fr.FileResourceID == id
                                               select fr).FirstOrDefault();

            if (fileResource != null)
            {
                dataContext.oxite_FileResources.DeleteOnSubmit(fileResource);
            }
        }

        public IStringResource GetString(string key, ILanguage language)
        {
            return getString(key, language.Name, null);
        }

        public IStringResource GetString(string key, ILanguage language, short version)
        {
            return getString(key, language.Name, version);
        }

        public IStringResource GetString(string key, string language)
        {
            return getString(key, language, null);
        }

        public IStringResource GetString(string key, string language, short version)
        {
            return getString(key, language, version);
        }

        public void SubmitChanges()
        {
            dataContext.SubmitChanges();
        }

        #endregion

        private IStringResource getString(string key, string language, short? version)
        {
            var query = from sr in dataContext.oxite_StringResources
                        where
                            string.Compare(sr.StringResourceKey, key, true) == 0 &&
                            (string.IsNullOrEmpty(language) || string.Compare(sr.Language, language, true) == 0)
                        select sr;

            if (version.HasValue)
            {
                query = query.Where(sr => sr.Version == version.Value);
            }

            return query.FirstOrDefault();
        }
    }
}