//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web.Mvc;

namespace Oxite.Mvc
{
    public class OxiteViewEngine : VirtualPathProviderViewEngine
    {
        private readonly string defaultSkin;

        public OxiteViewEngine(string skin)
        {
            defaultSkin = skin;

            MasterLocationFormats = new[]
                                    {
                                        "~/Skins/{2}/Views/{1}/{0}.master",
                                        "~/Skins/{2}/Views/Shared/{0}.master",
                                        "~/Views/{1}/{0}.master",
                                        "~/Views/Shared/{0}.master"
                                    };

            ViewLocationFormats = new[]
                                  {
                                      "~/Skins/{2}/Views/{1}/{0}.aspx",
                                      "~/Skins/{2}/Views/{1}/{0}.ascx",
                                      "~/Skins/{2}/Views/Shared/{0}.aspx",
                                      "~/Skins/{2}/Views/Shared/{0}.ascx",
                                      "~/Views/{1}/{0}.aspx",
                                      "~/Views/{1}/{0}.ascx",
                                      "~/Views/Shared/{0}.aspx",
                                      "~/Views/Shared/{0}.ascx",
                                  };

            PartialViewLocationFormats = ViewLocationFormats;
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new WebFormView(partialPath);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new WebFormView(viewPath, masterPath);
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            if (string.IsNullOrEmpty(partialViewName))
            {
                throw new ArgumentNullException("partialViewName");
            }

            string skinName = null; //todo(nheskew): where/how do user themes come into play?
            string controllerName = controllerContext.RouteData.GetRequiredString("controller");

            string[] locationsSearched;
            string partialPath = findPath(PartialViewLocationFormats, partialViewName, controllerName, skinName,
                                          out locationsSearched);

            return !string.IsNullOrEmpty(partialPath)
                       ? new ViewEngineResult(CreatePartialView(controllerContext, partialPath), this)
                       : new ViewEngineResult(locationsSearched);
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName,
                                                  string masterName)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentNullException("viewName");
            }

            string skinName = null; //todo(nheskew): where/how do user themes come into play?
            string controllerName = controllerContext.RouteData.GetRequiredString("controller");

            string[] viewLocationsSearched;
            string viewPath = findPath(ViewLocationFormats, viewName, controllerName, skinName,
                                       out viewLocationsSearched);

            if (string.IsNullOrEmpty(viewPath))
            {
                return new ViewEngineResult(viewLocationsSearched);
            }

            string[] masterLocationsSearched;
            string masterPath = findPath(MasterLocationFormats, masterName, controllerName, skinName,
                                         out masterLocationsSearched);

            if (!string.IsNullOrEmpty(masterName) && string.IsNullOrEmpty(masterPath))
            {
                return new ViewEngineResult(masterLocationsSearched);
            }

            return new ViewEngineResult(CreateView(controllerContext, viewPath, masterPath), this);
        }

        private string findPath(string[] locationsToSearch, string name, string controller, string requestedSkin,
                                out string[] locationsSearched)
        {
            int locationCount = 0;
            string skin = !string.IsNullOrEmpty(requestedSkin) ? requestedSkin : defaultSkin;
            locationsSearched = new string[locationsToSearch.Length];

            foreach (string locationFormat in locationsToSearch)
            {
                string path = string.Format(locationFormat, name, controller, skin);
                locationsSearched[locationCount++] = path;
                if (VirtualPathProvider.FileExists(path))
                {
                    return path;
                }
            }

            return null;
        }
    }
}