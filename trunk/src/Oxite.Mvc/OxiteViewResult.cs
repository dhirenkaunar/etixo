//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Linq;
using System.Web.Mvc;

namespace Oxite.Mvc
{
    public class OxiteViewResult : ViewResult
    {
        public new string MasterName
        {
            get
            {
                return !string.IsNullOrEmpty(base.MasterName) ? base.MasterName : "Site";
            }
            set
            {
                base.MasterName = value;
            }
        }

        protected override ViewEngineResult FindView(ControllerContext context)
        {
            ViewEngineResult result = ViewEngine.FindView(context, ViewName, MasterName);

            if (result.View != null)
            {
                return result;
            }

            throw new InvalidOperationException(
                string.Format(
                    "The view '{0}' could not be found. The following locations were searched:\r\n{1}",
                    ViewName,
                    string.Join("\r\n", result.SearchedLocations.ToArray())
                    )
                );
        }
    }
}