//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;

namespace Oxite.Mvc
{
    public class XmlViewResult : ViewResult
    {
        private string viewName;

        public XmlViewResult() : this("")
        {
        }

        public XmlViewResult(string viewName)
            : base()
        {
            this.viewName = viewName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (!string.IsNullOrEmpty(viewName))
            {
                ViewName = viewName;
            }

            TempData = context.Controller.TempData;
            ViewData = context.Controller.ViewData;

            base.ExecuteResult(context);

            context.HttpContext.Response.ContentType = "text/xml";
        }
    }
}