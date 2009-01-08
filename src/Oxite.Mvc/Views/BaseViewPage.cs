//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Mvc;
using Oxite.Configuration;
using Oxite.Data;
using Oxite.Mvc.Controllers;

namespace Oxite.Mvc.Views
{
    public class BaseViewPage : ViewPage
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

        public new IUser User
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ViewData["SkinPath"] = string.Format("/Skins/{0}/Content", Config.Site.ThemeDefault); //TODO: (erikpo) Change to be specified per user
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

        public virtual string RegisterScriptVariable(string name, object value)
        {
            DataContractJsonSerializer dcjs = new DataContractJsonSerializer(value.GetType());
            string script;

            using (MemoryStream ms = new MemoryStream())
            {
                dcjs.WriteObject(ms, value);

                script = string.Format("window.{0} = {1};", name, Encoding.Default.GetString(ms.ToArray()));

                ms.Close();
            }

            return script;
        }

        public virtual string RegisterSkinPathVariable()
        {
            return RegisterScriptVariable("skinPath", "/Content/skins/" + Config.Site.ThemeDefault);
                //TODO: (erikpo) Change to be specified per user
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

        public string GetAbsolutePath(string relativePath)
        {
            if (ViewContext.Controller is BaseController)
            {
                return ((BaseController)ViewContext.Controller).GetAbsolutePath(relativePath);
            }

            return null;
        }
    }
}