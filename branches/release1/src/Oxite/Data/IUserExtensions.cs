//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Routing;
using Oxite.Routing;

namespace Oxite.Data
{
    public static class IUserExtensions
    {
        public static string GetDisplayName(this IUser user)
        {
            if (!string.IsNullOrEmpty(user.DisplayName))
            {
                return user.DisplayName;
            }
            else
            {
                return user.Username;
            }
        }

        public static string GetUrl(this IUser user, RequestContext context, RouteCollection routes)
        {
            return routes.GetUrl(context, "UserProfile", new {username = user.Username});
        }
    }
}