//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Oxite.Routing;

namespace Oxite.Data
{
    public static class ICommentExtensions
    {
        public static string GetPermalinkHashValue(this DateTime commentDate)
        {
            return string.Format("c{0}", commentDate.ToString("yyyyMMddhhmmssf"));
        }

        public static IEnumerable<IComment> Visible(this IEnumerable<IComment> comments)
        {
            return comments.Where(c => c.State == (byte)EntityState.Normal);
        }

        public static string GetUrl(this IComment comment, RequestContext context, RouteCollection routes)
        {
            return routes.GetUrl(context, string.Format("{0}CommentPermalink", comment.Area.Type),
                                 new
                                 {
                                     areaName = comment.Area.Name,
                                     slug = comment.Post.Slug,
                                     comment = comment.Published.Value.GetPermalinkHashValue()
                                 });
        }
    }
}