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
    public class FakeUser : IUser
    {
        #region IUser Members

        public Guid ID { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string HashedEmail { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public byte Status { get; set; }
        public ILanguage LanguageDefault { get; set; }

        public bool IsAnonymous
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<ILanguage> Languages
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}