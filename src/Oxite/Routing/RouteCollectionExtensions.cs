//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web.Routing;

namespace Oxite.Routing
{
    public static class RouteCollectionExtensions
    {
        public static string GetUrl(this RouteCollection routes, RequestContext context, string route, object values)
        {
            VirtualPathData pathData = routes.GetVirtualPath(context, route, new RouteValueDictionary(values));

            if (pathData == null)
            {
                return "";
            }
            else
            {
                return pathData.VirtualPath;
            }
        }

        public static string GetAbsoluteUrl(this RouteCollection routes, RequestContext context, string route,
                                            object values)
        {
            string url = routes.GetUrl(context, route, values);

            if (url != "")
            {
                Uri uri = context.HttpContext.Request.Url;
                UriBuilder uriBuilder = new UriBuilder(uri.Scheme, uri.Host, uri.Port);

                uriBuilder.Path = url;

                url = uriBuilder.Uri.ToString();
            }

            return url;
        }
    }
}