//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Oxite.Configuration;
using Oxite.Data;

namespace Oxite.Mvc.Controllers
{
    public class SEOController : BaseController
    {
        public SEOController()
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
        }

        public SEOController(RouteCollection routes, IOxiteConfiguration config, NameValueCollection appSettings,
                             IAreaRepository areaRepository, IMembershipRepository membershipRepository,
                             IPostRepository postRepository, IResourceRepository resourceRepository)
            : base(routes, config, appSettings, areaRepository, membershipRepository, postRepository, resourceRepository
                )
        {
        }

        public virtual ActionResult Index()
        {
            ViewData["YearMonths"] = PostRepository.GetYearMonthsOfPosts(Config.Site.ID);

            return Xml();
        }

        public virtual ActionResult Permalink(int year, int month)
        {
            DateTime startDate = new DateTime(year, month, 1);

            ViewData["Posts"] = PostRepository.GetPosts(Config.Site.ID, startDate, startDate.AddMonths(1));

            return Xml();
        }

        public virtual ActionResult Robots()
        {
            ViewData["SiteMap"] = Url.RouteUrl("SiteMapIndex");

            //TODO: (erikpo) Add other stuff to ViewData like which paths and/or pages shouldn't be indexed, etc

            return View();
        }
    }
}