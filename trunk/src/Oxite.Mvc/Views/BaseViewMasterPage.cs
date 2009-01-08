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
    public class BaseViewMasterPage : ViewMasterPage
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
    }
}