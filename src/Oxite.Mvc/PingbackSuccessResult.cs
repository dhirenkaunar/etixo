//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;

namespace Oxite.Mvc
{
    public class PingbackSuccessResult : TrackbackResult
    {
        private string sourceUrl;
        private string targetUrl;

        public PingbackSuccessResult(string sourceUrl, string targetUrl) : this("Success", sourceUrl, targetUrl)
        {
        }

        public PingbackSuccessResult(string viewName, string sourceUrl, string targetUrl)
            : base(viewName)
        {
            this.sourceUrl = sourceUrl;
            this.targetUrl = targetUrl;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.Controller.ViewData["SourceUrl"] = sourceUrl;
            context.Controller.ViewData["TargetUrl"] = targetUrl;

            base.ExecuteResult(context);
        }
    }
}