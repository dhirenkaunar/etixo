//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Web.Mvc;
using Oxite.Configuration;
using Oxite.Data;
using Oxite.Mvc.Controllers;

namespace Oxite.Mvc.Views
{
    public class BaseViewUserControl : ViewUserControl
    {
        public bool IsDebugging
        {
            get
            {
                if (ViewContext.Controller is BaseController)
                {
                    return ((BaseController)ViewContext.Controller).IsDebugging;
                }

                return false;
            }
        }

        public IOxiteConfiguration Config
        {
            get
            {
                if (ViewContext.Controller is BaseController)
                {
                    return ((BaseController)ViewContext.Controller).Config;
                }

                return null;
            }
        }

        public AppSettingsHelper AppSettings
        {
            get
            {
                if (ViewContext.Controller is BaseController)
                {
                    return ((BaseController)ViewContext.Controller).AppSettings;
                }

                return null;
            }
        }

        public PageTitleHelper PageTitle
        {
            get
            {
                if (ViewContext.Controller is BaseController)
                {
                    return ((BaseController)ViewContext.Controller).PageTitle;
                }

                return null;
            }
        }

        public IUser User
        {
            get
            {
                if (ViewContext.Controller is BaseController)
                {
                    return ((BaseController)ViewContext.Controller).User;
                }

                return null;
            }
        }

        public DateTime ConvertToLocalTime(DateTime dateTime)
        {
            if (ViewContext.Controller is BaseController)
            {
                return ((BaseController)ViewContext.Controller).ConvertToLocalTime(dateTime);
            }

            return dateTime;
        }

        public virtual string RegisterCssFile(string path)
        {
            return RegisterCssFile(path, null);
        }

        public virtual string RegisterCssFile(string path, string releasePath)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

#if DEBUG
#else
            if (!string.IsNullOrEmpty(releasePath))
                path = releasePath;
#endif

            if (!(path.StartsWith("http://") || path.StartsWith("https://")))
            {
                if (!path.StartsWith("/"))
                {
                    path = "/" + path;
                }

                //TODO: (erikpo) When there is more than one theme and they are user selectable, change from using the site default theme to the current user's selected theme
                path = string.Format(Config.Site.CssPath, Config.Site.ThemeDefault) + path;
            }

            return Html.HeadLink("stylesheet", path, "text/css", "");
        }

        public virtual string RegisterScript(string path)
        {
            return RegisterScript(path, null);
        }

        public virtual string RegisterScript(string path, string releasePath)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

#if DEBUG
#else
            if (!string.IsNullOrEmpty(releasePath))
                path = releasePath;
#endif

            if (!(path.StartsWith("http://") || path.StartsWith("https://")))
            {
                if (!path.StartsWith("/"))
                {
                    path = "/" + path;
                }

                //TODO: (erikpo) When there is more than one theme and they are user selectable, change from using the site default theme to the current user's selected theme
                path = string.Format(Config.Site.ScriptsPath, Config.Site.ThemeDefault) + path;
            }

            return Html.ScriptBlock("text/javascript", path);
        }

        public string Localize(string value)
        {
            return Localize(value, false);
        }

        public string Localize(string value, string ns)
        {
            return Localize(value, ns, false);
        }

        public string Localize(string value, bool jsonSerialize)
        {
            return Localize(value, "", jsonSerialize);
        }

        public string Localize(string value, string ns, bool jsonSerialize)
        {
            if (ViewContext.Controller is BaseController)
            {
                //TODO: (erikpo) Render the following localized text into a client control that lets admins add new translations for this string resource
                return ((BaseController)ViewContext.Controller).Localize(value, ns, jsonSerialize);
            }

            return null;
        }

        public void RenderStringResources()
        {
            if (ViewContext.Controller is BaseController)
            {
                ((BaseController)ViewContext.Controller).RenderStringResources();
            }
        }
    }
}