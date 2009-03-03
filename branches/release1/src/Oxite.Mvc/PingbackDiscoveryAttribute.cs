//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Web.Mvc;
using Oxite.Data;
using Oxite.Mvc.Controllers;

namespace Oxite.Mvc
{
    public class PingbackDiscoveryAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewResult viewResult = filterContext.Result as ViewResult;

            base.OnActionExecuted(filterContext);

            if (viewResult != null && viewResult.ViewData["Post"] != null) // to let us 404
            {
                if (!(filterContext.Controller is BaseController))
                {
                    throw new ArgumentException(
                        string.Format("This attribute can only be used on a Controller of type '{0}'",
                                      typeof (BaseController).FullName));
                }

                BaseController controller = ((BaseController)filterContext.Controller);

                if (controller.Config.Site.TrackbacksEnabled)
                {
                    IPost post = (IPost)viewResult.ViewData["Post"];

                    if (post == null)
                    {
                        throw new ArgumentException("ViewData item 'Post' could not be found and is required");
                    }

                    Uri uri = filterContext.HttpContext.Request.Url;
                    UriBuilder uriBuilder = new UriBuilder(uri.Scheme, uri.Host, uri.Port);

                    uriBuilder.Path = controller.Url.RouteUrl("Pingback", new {id = post.ID.ToString("N")});

                    viewResult.ViewData["Pingback.Url"] = uriBuilder.Uri.ToString();
                }
            }
        }
    }
}