//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;

namespace Oxite.Data
{
    public interface IComment
    {
        Guid PostID { get; set; }
        IPost Post { get; }
        Guid ID { get; set; }
        Guid CreatorUserID { get; set; }
        IUser CreatorUser { get; }
        Guid LanguageID { get; set; }
        ILanguage Language { get; }
        long CreatorIP { get; set; }
        string UserAgent { get; set; }
        byte State { get; set; }
        DateTime? Created { get; set; }
        DateTime? Modified { get; set; }
        string Title { get; }
        string Body { get; set; }
        DateTime? Published { get; set; }
        string CreatorName { get; }
        string CreatorEmail { get; }
        string CreatorHashedEmail { get; }
        string CreatorUrl { get; }
        IArea Area { get; }
        IEnumerable<ITag> Tags { get; }
    }
}