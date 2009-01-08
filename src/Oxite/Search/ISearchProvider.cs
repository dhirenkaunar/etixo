//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.Collections.Specialized;
using Oxite.Data;

namespace Oxite.Search
{
    public interface ISearchProvider
    {
        PageOfAList<ISearchResultItem> GetSearchResults(string term, int page, int count, NameValueCollection parameters);
    }
}