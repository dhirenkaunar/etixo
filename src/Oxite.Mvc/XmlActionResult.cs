//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;
using System.Xml.Linq;

namespace Oxite.Mvc
{
    public class XmlActionResult : ActionResult
    {
        private XDocument document;

        public XmlActionResult(XDocument document)
            : base()
        {
            this.document = document;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml";
            document.Save(context.HttpContext.Response.Output, SaveOptions.DisableFormatting);
        }
    }
}