//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Web.Mvc;
using Oxite.Mvc.Controllers;

namespace Oxite.Mvc
{
    public class ValidateAntiForgeryTokenAttribute : FilterAttribute, IAuthorizationFilter
    {
        private string salt;

        public string Salt
        {
            get
            {
                return salt ?? string.Empty;
            }
            set
            {
                salt = value;
            }
        }

        #region IAuthorizationFilter Members

        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            BaseController controller = (BaseController)filterContext.Controller;
            string ticksValue = filterContext.HttpContext.Request.Form[AntiForgeryToken.TicksInputName];
            string tokenHash = filterContext.HttpContext.Request.Form[AntiForgeryToken.TokenInputName];
            long ticks;

            if (string.IsNullOrEmpty(ticksValue) || !long.TryParse(ticksValue, out ticks))
            {
                throw new ValidateAntiForgeryTokenException(
                    string.Format(controller.Localize("Missing AntiForgery token form param: {0}"),
                                  AntiForgeryToken.TicksInputName));
            }

            if (string.IsNullOrEmpty(tokenHash))
            {
                throw new ValidateAntiForgeryTokenException(
                    string.Format(controller.Localize("Missing AntiForgery token form param: {0}"),
                                  AntiForgeryToken.TokenInputName));
            }

            AntiForgeryToken token = new AntiForgeryToken(controller, ticks.ToString());
            TimeSpan timeOffset = new TimeSpan(DateTime.Now.Ticks - ticks);

            //todo(nheskew): drop the time span into some configurable property
            if (!(token.GetHash(Salt) == tokenHash && timeOffset.TotalMinutes < 360))
            {
                throw new ValidateAntiForgeryTokenException();
            }
        }

        #endregion
    }
}