//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using Oxite.Data;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeTag : ITag
    {
        #region ITag Members

        public ITag Parent
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Guid ID { get; set; }
        public string Name { get; set; }
        public DateTime? Created { get; set; }

        #endregion
    }
}