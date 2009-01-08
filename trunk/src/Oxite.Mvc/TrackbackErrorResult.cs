//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;

namespace Oxite.Mvc
{
    public class TrackbackErrorResult : TrackbackResult
    {
        private int errorCode;
        private string errorText;

        public TrackbackErrorResult(int errorCode, string errorText) : this("Error", errorCode, errorText)
        {
        }

        public TrackbackErrorResult(string viewName, int code, string text)
            : base(viewName)
        {
            this.errorCode = code;
            this.errorText = text;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            //TODO: (erikpo) Log error

            ViewData["ErrorCode"] = errorCode;
            ViewData["ErrorText"] = errorText;

            base.ExecuteResult(context);
        }
    }
}