//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System.Web.Mvc;
using Oxite.Data;

namespace Oxite.Mvc
{
    public class FileResult : ActionResult
    {
        private IFileResource fileResource;

        public FileResult(IFileResource fileResource)
        {
            this.fileResource = fileResource;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = fileResource.ContentType;
            context.HttpContext.Response.BinaryWrite(fileResource.Content);
        }
    }
}