//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;
using System.Web.UI;
using Oxite.Mvc.Controllers;

namespace Oxite.Mvc
{
    using Services;
    using ViewModel.Security;

    [HandleError]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class AccountController : BaseController
    {
        private readonly IAuthenticationService authenticationService;

        public AccountController() : this(new AuthenticationService())
        {
        }

        public AccountController(IAuthenticationService service)
        {
            authenticationService = service;
        }

        public ActionResult SignIn()
        {
            PageTitle.AdditionalPageTitleSegments = new string[] { "Sign In" };

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SignIn(CredentialViewModel viewModel, string returnUrl)
        {
            PageTitle.AdditionalPageTitleSegments = new[] { "Sign In" };

            var result = authenticationService.Authenticate(viewModel);

            if (result.Status == AuthenticationStatus.Success)
            {
                authenticationService.SetSecurityContext(viewModel);

                if (!string.IsNullOrEmpty(returnUrl) && returnUrl.StartsWith("/"))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToRoute("Home");
            }

            ViewData["rememberMe"] = viewModel.Persist;

            return View();
        }

        public ActionResult SignOut()
        {
            authenticationService.Logout();
            return RedirectToRoute("Home");
        }
    }
}