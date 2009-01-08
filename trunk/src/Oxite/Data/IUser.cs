//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;

namespace Oxite.Data
{
    public interface IUser
    {
        Guid ID { get; set; }
        string Username { get; set; }
        string DisplayName { get; set; }
        string Email { get; set; }
        string HashedEmail { get; set; }
        string Password { get; set; }
        string PasswordSalt { get; set; }
        byte Status { get; set; }
        bool IsAnonymous { get; }
        ILanguage LanguageDefault { get; set; }
        IEnumerable<ILanguage> Languages { get; }
    }
}