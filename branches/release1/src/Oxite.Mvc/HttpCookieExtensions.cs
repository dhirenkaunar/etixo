//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Web;
using Oxite.Data;

namespace Oxite.Mvc
{
    public static class HttpCookieExtensions
    {
        public static ICommentAnonymous ToCommentAnonymous(this HttpCookie cookie, Func<Type> getType)
        {
            return cookie != null ? cookie.Value.ToCommentAnonymous(getType) : null;
        }
    }
}