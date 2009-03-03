//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;

namespace Oxite.Mvc
{
    public abstract class TrackbackResult : ViewResult
    {
        protected string viewName;

        public TrackbackResult(string viewName)
            : base()
        {
            this.viewName = viewName;
        }

        protected virtual string GetViewName(ControllerContext context)
        {
            return context.RouteData.GetRequiredString("action") + viewName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            ViewName = GetViewName(context);

            base.ExecuteResult(context);

            context.HttpContext.Response.ContentType = "text/xml";
        }
    }
}