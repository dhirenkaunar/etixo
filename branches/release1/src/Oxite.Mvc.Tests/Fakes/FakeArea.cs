//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using Oxite.Data;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeArea : IArea
    {
        #region IArea Members

        public Guid SiteID { get; set; }
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string TypeUrl { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        #endregion
    }
}