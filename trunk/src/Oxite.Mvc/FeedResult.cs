//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Oxite.Data;

namespace Oxite.Mvc
{
    public class FeedResult : ViewResult
    {
        private IEnumerable<IFeedItem> feedItems;
        private string viewName;

        public FeedResult(IEnumerable<IFeedItem> feedItems) : this(feedItems, "")
        {
        }

        public FeedResult(IEnumerable<IFeedItem> feedItems, string viewName)
            : base()
        {
            this.feedItems = feedItems;
            this.viewName = viewName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            string modifiedSince = context.HttpContext.Request.Headers["If-Modified-Since"];
            bool isClientCached = false;

            if (!string.IsNullOrEmpty(modifiedSince))
            {
                DateTime modifiedSinceDate;

                if (DateTime.TryParse(modifiedSince, out modifiedSinceDate))
                {
                    feedItems = feedItems.ToList().Where(items => items.Published > modifiedSinceDate.ToUniversalTime());
                }

                if (feedItems.Count() == 0)
                {
                    isClientCached = true;
                }
            }

            TempData = context.Controller.TempData;
            ViewData = context.Controller.ViewData;

            ViewData["FeedItems"] = feedItems;

            if (!string.IsNullOrEmpty(viewName))
            {
                ViewName = viewName;
            }

            base.ExecuteResult(context);

            if (!isClientCached)
            {
                context.HttpContext.Response.ContentType = "application/xml";
            }
            else
            {
                context.HttpContext.Response.StatusCode = 304;
                context.HttpContext.Response.SuppressContent = true;
            }
        }
    }
}