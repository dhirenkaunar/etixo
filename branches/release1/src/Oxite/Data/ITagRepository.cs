//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;

namespace Oxite.Data
{
    public interface ITagRepository
    {
        IEnumerable<ITag> GetTags();
        IEnumerable<KeyValuePair<ITag, int>> GetTagsWithPostCount();
        ITag GetTag(string name);
        ITag GetTag(Guid id);
        ITag CreateTag();
        ITag CreateTag(string name);
        void AddTag(ITag tag);
        void SubmitChanges();
    }
}