using System;
using System.Web.Mvc;

using Oxite.Configuration;
using Oxite.Data;
using Oxite.Mvc;
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
                    return ((BaseController)ViewContext.Controller).IsDebugging;
                else
                    return false;
            }
        }

        public IOxiteConfiguration Config
        {
            get
            {
                if (ViewContext.Controller is BaseController)
                    return ((BaseController)ViewContext.Controller).Config;
                else
                    return null;
            }
        }

        public AppSettingsHelper AppSettings
        {
            get
            {
                if (ViewContext.Controller is BaseController)
                    return ((BaseController)ViewContext.Controller).AppSettings;
                else
                    return null;
            }
        }

        public PageTitleHelper PageTitle
        {
            get
            {
                if (ViewContext.Controller is BaseController)
                    return ((BaseController)ViewContext.Controller).PageTitle;
                else
                    return null;
            }
        }

        public DateTime ConvertToLocalTime(DateTime dateTime)
        {
            if (ViewContext.Controller is BaseController)
                return ((BaseController)ViewContext.Controller).ConvertToLocalTime(dateTime);
            else
                return dateTime;
        }

        public IUser User
        {
            get
            {
                if (ViewContext.Controller is BaseController)
                    return ((BaseController)ViewContext.Controller).User;
                else
                    return null;
            }
        }
    }
}
