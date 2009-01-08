//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oxite.Data
{
    internal static class PostExtensions
    {
        internal static IQueryable<oxite_Post> Visible(this IQueryable<oxite_Post> posts)
        {
            return posts.Published().Where(p => p.State == (byte)EntityState.Normal);
        }

        internal static IQueryable<oxite_Post> Published(this IQueryable<oxite_Post> posts)
        {
            return posts.Where(e => e.PublishedDate < DateTime.Now.ToUniversalTime());
        }

        internal static IQueryable<oxite_Post> InYear(this IQueryable<oxite_Post> posts, int year)
        {
            return year == 0 ? posts : posts.Where(e => e.PublishedDate.Year == year);
        }

        internal static IQueryable<oxite_Post> InMonth(this IQueryable<oxite_Post> posts, int month)
        {
            return month == 0 ? posts : posts.Where(e => e.PublishedDate.Month == month);
        }

        internal static IQueryable<oxite_Post> InDay(this IQueryable<oxite_Post> posts, int? day)
        {
            return day == 0 ? posts : posts.Where(e => e.PublishedDate.Day == day);
        }

        internal static IEnumerable<KeyValuePair<DateTime, int>> ArchiveList(this IQueryable<oxite_Post> posts)
        {
            if (posts != null)
            {
                return
                    posts.OrderByDescending(e => e.PublishedDate).GroupBy(
                        e => new DateTime(e.PublishedDate.Year, e.PublishedDate.Month, 1)).Select(
                        g => new KeyValuePair<DateTime, int>(g.Key, g.Count()));
            }
            else
            {
                return null;
            }
        }
    }
}