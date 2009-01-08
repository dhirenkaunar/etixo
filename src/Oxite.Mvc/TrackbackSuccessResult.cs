//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Web.Mvc;
using Oxite.Data;

namespace Oxite.Mvc
{
    public class TrackbackSuccessResult : TrackbackResult
    {
        protected IPost post;

        public TrackbackSuccessResult(IPost post) : this(post, "Success")
        {
        }

        public TrackbackSuccessResult(IPost post, string viewName)
            : base(viewName)
        {
            this.post = post;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            Controller controller = ((Controller)context.Controller);
            Uri uri = context.HttpContext.Request.Url;
            UriBuilder builder = new UriBuilder(uri.Scheme, uri.Host, uri.Port);

            builder.Path = controller.Url.RouteUrl("Trackback", new {id = post.ID.ToString("N")});

            ViewData["Url"] = builder.Uri.ToString();
            ViewData["Post"] = post;

            base.ExecuteResult(context);
        }
    }
}