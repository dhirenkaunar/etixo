//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI;
using Oxite.Configuration;
using Oxite.Data;
using Oxite.Mvc.Controllers;

namespace Oxite.Mvc
{
    [HandleError]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class AccountController : BaseController
    {
        public AccountController()
        {
            Routes = RouteTable.Routes;
            Config = (IOxiteConfiguration)ConfigurationManager.GetSection("oxite");
            AppSettings = new AppSettingsHelper(ConfigurationManager.AppSettings);
            PageTitle = new PageTitleHelper(Config);

            IOxiteDataProvider dataProvider = Config.DataProvider.GetInstance();
            AreaRepository = dataProvider.AreaRepository;
            MembershipRepository = dataProvider.MembershipRepository;
            PostRepository = dataProvider.PostRepository;
            ResourceRepository = dataProvider.ResourceRepository;

            FormsAuth = new FormsAuthenticationWrapper();
        }

        public AccountController(RouteCollection routes, IOxiteConfiguration config, NameValueCollection appSettings,
                                 IAreaRepository areaRepository, IMembershipRepository membershipRepository,
                                 IPostRepository postRepository, IResourceRepository resourceRepository,
                                 IFormsAuthentication formsAuth)
            : base(routes, config, appSettings, areaRepository, membershipRepository, postRepository, resourceRepository
                )
        {
            FormsAuth = formsAuth;
        }

        public IFormsAuthentication FormsAuth { get; protected set; }

        public ActionResult SignIn()
        {
            PageTitle.AdditionalPageTitleSegments = new string[] {"Sign In"};

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SignIn(string username, string password, bool rememberMe, string returnUrl)
        {
            PageTitle.AdditionalPageTitleSegments = new string[] {"Sign In"};

            if (string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("username", "You must specify a username.");
            }

            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", "You must specify a password.");
            }

            if (ViewData.ModelState.IsValid)
            {
                IUser user = MembershipRepository.GetUser(username, password);

                if (user != null)
                {
                    FormsAuth.SetAuthCookie(username, rememberMe);

                    if (!string.IsNullOrEmpty(returnUrl) && returnUrl.StartsWith("/"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToRoute("Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("_FORM", "The username or password provided is incorrect.");
                }
            }

            ViewData["rememberMe"] = rememberMe;

            return View();
        }

        public ActionResult SignOut()
        {
            FormsAuth.SignOut();

            return RedirectToRoute("Home");
        }

        // The FormsAuthentication type is sealed and contains static members, so it is difficult to
        // unit test code that calls its members. The interface and helper class below demonstrate
        // how to create an abstract wrapper around such a type in order to make the AccountController
        // code unit testable.

        #region Nested type: FormsAuthenticationWrapper

        public class FormsAuthenticationWrapper : IFormsAuthentication
        {
            #region IFormsAuthentication Members

            public void SetAuthCookie(string userName, bool createPersistentCookie)
            {
                FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
            }

            public void SignOut()
            {
                FormsAuthentication.SignOut();
            }

            #endregion
        }

        #endregion

        #region Nested type: IFormsAuthentication

        public interface IFormsAuthentication
        {
            void SetAuthCookie(string userName, bool createPersistentCookie);
            void SignOut();
        }

        #endregion
    }
}