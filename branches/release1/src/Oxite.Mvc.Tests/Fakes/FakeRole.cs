//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using Oxite.Data;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeRole : IRole
    {
        #region IRole Members

        public IRole Parent
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Guid ID { get; set; }
        public string Name { get; set; }

        #endregion
    }
}