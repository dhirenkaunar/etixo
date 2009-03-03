//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.Data
{
    public interface IFileResource
    {
        Guid SiteID { get; set; }
        Guid ID { get; set; }
        string Name { get; set; }
        IUser CreatorUser { get; set; }
        byte[] Content { get; set; }
        string ContentType { get; set; }
        string Path { get; set; }
        byte State { get; set; }
        DateTime? Created { get; set; }
        DateTime? Modified { get; set; }
    }
}