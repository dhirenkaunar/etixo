//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.Web.Mvc;
using Oxite.Data;

namespace Oxite.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string User(this UrlHelper urlHelper, IUser user)
        {
            return user.GetUrl(urlHelper.RequestContext, urlHelper.RouteCollection);
        }

        public static string Area(this UrlHelper urlHelper, IArea area)
        {
            return area.GetUrl(urlHelper.RequestContext, urlHelper.RouteCollection);
        }

        public static string AdminArea(this UrlHelper urlHelper, IArea area)
        {
            return area.GetAdminUrl(urlHelper.RequestContext, urlHelper.RouteCollection);
        }

        public static string Tag(this UrlHelper urlHelper, ITag tag)
        {
            return tag.GetUrl(urlHelper.RequestContext, urlHelper.RouteCollection);
        }

        public static string Post(this UrlHelper urlHelper, IPost post)
        {
            return post.GetAreaPostUrl(urlHelper.RequestContext, urlHelper.RouteCollection);
        }

        public static string Page(this UrlHelper urlHelper, IPost post)
        {
            return post.GetPageUrl(urlHelper.RequestContext, urlHelper.RouteCollection);
        }

        public static string Comment(this UrlHelper urlHelper, IComment comment)
        {
            return comment.GetUrl(urlHelper.RequestContext, urlHelper.RouteCollection);
        }
    }
}