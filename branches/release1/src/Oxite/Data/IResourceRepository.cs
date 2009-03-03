//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;

namespace Oxite.Data
{
    public interface IResourceRepository
    {
        IFileResource GetFile(Guid siteID, Guid userID, Guid fileResourceID);
        IFileResource GetFile(Guid siteID, Guid userID, string filePath);
        IFileResource GetFile(Guid siteID, string path, string name);
        IEnumerable<IFileResource> GetFiles(Guid siteID, Guid userID);
        //INFO: (erikpo) Might need paging if the list of files gets too long
        IFileResource CreateFile();
        void AddFile(IFileResource fileResource);
        void AddFileToUser(Guid fileResourceID, Guid userID);
        void RemoveFileFromUser(Guid fileResourceID, Guid userID);
        void RemoveFile(Guid id);

        IStringResource GetString(string key, string language, short version);
        IStringResource GetString(string key, string language);
        IStringResource GetString(string key, ILanguage language, short version);
        IStringResource GetString(string key, ILanguage language);

        void SubmitChanges();
    }
}