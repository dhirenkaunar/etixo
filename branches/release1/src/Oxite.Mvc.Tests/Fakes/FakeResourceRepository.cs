//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Generic;
using Oxite.Data;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeResourceRepository : IResourceRepository
    {
        public FakeResourceRepository()
        {
            AddedFiles = new List<FakeFileResource>();
            AddedUserFiles = new List<KeyValuePair<Guid, Guid>>();
            Saved = false;
        }

        public List<FakeFileResource> AddedFiles { get; set; }
        public List<KeyValuePair<Guid, Guid>> AddedUserFiles { get; set; }
        public bool Saved { get; set; }

        #region IResourceRepository Members

        public IFileResource GetFile(Guid siteID, Guid userID, Guid fileResourceID)
        {
            throw new NotImplementedException();
        }

        public IFileResource GetFile(Guid siteID, Guid userID, string filePath)
        {
            throw new NotImplementedException();
        }

        public IFileResource GetFile(Guid siteID, string path, string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFileResource> GetFiles(Guid siteID, Guid userID)
        {
            throw new NotImplementedException();
        }

        public IFileResource CreateFile()
        {
            return new FakeFileResource();
        }

        public void AddFile(IFileResource fileResource)
        {
            AddedFiles.Add((FakeFileResource)fileResource);
        }

        public void AddFileToUser(Guid fileResourceID, Guid userID)
        {
            AddedUserFiles.Add(new KeyValuePair<Guid, Guid>(userID, fileResourceID));
        }

        public void RemoveFileFromUser(Guid fileResourceID, Guid userID)
        {
            throw new NotImplementedException();
        }

        public void RemoveFile(Guid id)
        {
            throw new NotImplementedException();
        }

        public IStringResource GetString(string key, string language, short version)
        {
            throw new NotImplementedException();
        }

        public IStringResource GetString(string key, string language)
        {
            throw new NotImplementedException();
        }

        public IStringResource GetString(string key, ILanguage language, short version)
        {
            throw new NotImplementedException();
        }

        public IStringResource GetString(string key, ILanguage language)
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