//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Routing;
using Oxite.Routing;

namespace Oxite.Data
{
    public static class IPostExtensions
    {
        private static Regex tags = new Regex("<[^<>]*>", RegexOptions.Compiled);

        public static string GetBodyShort(this IPost post)
        {
            return !string.IsNullOrEmpty(post.BodyShort)
                       ? post.BodyShort
                       : post.GetBodyShort(100);
        }

        public static string GetBodyShort(this IPost post, int wordCount)
        {
            string previewText = !string.IsNullOrEmpty(post.Body)
                                     ? tags.Replace(post.Body, "").CleanWhitespace()
                                     : string.Empty;

            if (!string.IsNullOrEmpty(previewText))
            {
                previewText = string.Join(" ", previewText.Split(' ').Take(wordCount).ToArray());
            }

            return previewText;
        }

        public static string GetAreaPostUrl(this IPost post, RequestContext context, RouteCollection routes)
        {
            return post.GetAreaPostUrl("", context, routes);
        }

        public static string GetAreaPostUrl(this IPost post, string routeNameFormat, RequestContext context,
                                            RouteCollection routes)
        {
            string routeName = string.Format("{0}Permalink", post.Area.Type);

            if (!string.IsNullOrEmpty(routeNameFormat))
            {
                routeName = string.Format(routeNameFormat, routeName);
            }

            return routes.GetUrl(context, routeName, new {areaName = post.Area.Name, slug = post.Slug});
        }

        public static string GetPageUrl(this IPost post, RequestContext context, RouteCollection routes)
        {
            StringBuilder pagePath = new StringBuilder(100);
            IPost currentPost = post;

            pagePath.Append(currentPost.Slug);

            while (currentPost.Parent.ID != currentPost.ID)
            {
                currentPost = currentPost.Parent;

                pagePath.Insert(0, string.Format("{0}/", currentPost.Slug));
            }

            return routes.GetUrl(context, "Page", new {pagePath = pagePath.ToString()});
        }
    }
}