//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;

namespace Oxite.Data
{
    public interface ILanguageRepository
    {
        ILanguage GetLanguage(Guid id);
        ILanguage GetLanguage(string name);
        IEnumerable<ILanguage> GetLanguages();
        IEnumerable<ILanguage> GetLanguages(IUser user);
    }
}