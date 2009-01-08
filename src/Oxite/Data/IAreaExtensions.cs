//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Routing;
using Oxite.Routing;

namespace Oxite.Data
{
    public static class IAreaExtensions
    {
        public static string GetUrl(this IArea area, RequestContext context, RouteCollection routes)
        {
            return area.GetUrl("", context, routes);
        }

        public static string GetUrl(this IArea area, string routeNameFormat, RequestContext context,
                                    RouteCollection routes)
        {
            string routeName = area.Type;

            if (!string.IsNullOrEmpty(routeNameFormat))
            {
                routeName = string.Format(routeNameFormat, routeName);
            }

            return routes.GetUrl(context, routeName, new {areaName = area.Name});
        }

        public static string GetAdminUrl(this IArea area, RequestContext context, RouteCollection routes)
        {
            return routes.GetUrl(context, "AdminAreasEdit", new {areaID = area.ID});
        }
    }
}