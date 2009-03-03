//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Linq;

namespace Oxite.Data
{
    public static class IQueryableExtensions
    {
        public static PageOfAList<IPost> GetPage(this IQueryable<oxite_Post> list, int pageIndex, int pageSize)
        {
            return GetPage<oxite_Post, IPost>(list, pageIndex, pageSize);
        }

        public static PageOfAList<TInterface> GetPage<TEntity, TInterface>(this IQueryable<TEntity> list, int pageIndex,
                                                                           int pageSize)
        {
            if (list != null)
            {
                return new PageOfAList<TInterface>(list.Skip(pageIndex * pageSize).Take(pageSize).Cast<TInterface>(),
                                                   pageIndex, pageSize, list.Count());
            }

            return null;
        }
    }
}