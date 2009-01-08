//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.Collections.Generic;
using System.Linq;
using Oxite.Data;

namespace Oxite.Search
{
    public class LiveSearchResults : PageOfAList<ISearchResultItem>
    {
        public LiveSearchResults(IEnumerable<LiveSearchResultItem> items, List<string> suggestions, int pageIndex,
                                 int pageSize, int totalItemCount)
            : base(items.Cast<ISearchResultItem>(), pageIndex, pageSize, totalItemCount)
        {
            Suggestions = suggestions;
        }

        public List<string> Suggestions { get; private set; }
    }
}