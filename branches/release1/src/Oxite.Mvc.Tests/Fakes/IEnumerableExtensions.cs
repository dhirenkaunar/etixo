//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.Collections.Generic;
using System.Linq;
using Oxite.Data;

namespace Oxite.Mvc.Tests.Fakes
{
    public static class IEnumerableExtensions
    {
        public static PageOfAList<IPost> GetPage(this IEnumerable<FakePost> list, int pageIndex, int pageSize)
        {
            return GetPage<FakePost, IPost>(list, pageIndex, pageSize);
        }

        public static PageOfAList<TInterface> GetPage<TEntity, TInterface>(this IEnumerable<TEntity> list, int pageIndex,
                                                                           int pageSize)
        {
            if (list != null)
            {
                return new PageOfAList<TInterface>(list.Skip(pageIndex * pageSize).Take(pageSize).Cast<TInterface>(),
                                                   pageIndex, pageSize, list.Count());
            }
            else
            {
                return null;
            }
        }
    }
}