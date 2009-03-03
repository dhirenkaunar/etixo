//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using Oxite.Data;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeFileResource : IFileResource
    {
        #region IFileResource Members

        public Guid SiteID { get; set; }
        public Guid ID { get; set; }
        public string Name { get; set; }
        public IUser CreatorUser { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
        public string Path { get; set; }
        public byte State { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        #endregion
    }
}