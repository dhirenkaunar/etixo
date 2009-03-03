//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Oxite.Configuration;
using Oxite.Data;

namespace Oxite.Mvc.Controllers
{
    public class ArchiveController : BaseController
    {
        public ArchiveController()
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

        public ArchiveController(RouteCollection routes, IOxiteConfiguration config, NameValueCollection appSettings,
                                 IAreaRepository areaRepository, IMembershipRepository membershipRepository,
                                 IPostRepository postRepository, IResourceRepository resourceRepository)
            : base(routes, config, appSettings, areaRepository, membershipRepository, postRepository, resourceRepository
                )
        {
        }

        public virtual ActionResult Index([ModelBinder(typeof (ArchiveDataBinder))] ArchiveData archivaData,
                                          string areaName)
        {
            IArea area = AreaRepository.GetArea(areaName);
            IPageOfAList<IPost> posts = area != null
                                            ? PostRepository.GetPosts(area, archivaData.Year, archivaData.Month,
                                                                      archivaData.Day, archivaData.Page - 1, 10)
                                            : PostRepository.GetPosts(Config.Site.ID, archivaData.Year,
                                                                      archivaData.Month, archivaData.Day,
                                                                      archivaData.Page - 1, 10);
            Dictionary<Guid, int> postCounts = new Dictionary<Guid, int>(posts.Count);
            List<string> titles = new List<string>(5);

            titles.Add("Archives");
            if (area != null)
            {
                titles.Add(!string.IsNullOrEmpty(area.DisplayName) ? area.DisplayName : area.Name);
            }
            titles.Add(archivaData.Year.ToString());
            if (archivaData.Month > 0)
            {
                titles.Add(new DateTime(archivaData.Year, archivaData.Month, 1).ToString("MMMM"));

                if (archivaData.Day > 0)
                {
                    titles.Add(archivaData.Day.ToString());
                }
            }

            PageTitle.AdditionalPageTitleSegments = titles.ToArray();

            posts.ToList().ForEach(p => postCounts.Add(p.ID, PostRepository.GetComments(p).Count()));

            ViewData["AreaCount"] = AreaRepository.GetAreasCount(Config.Site.ID);
            ViewData["Area"] = area;
            ViewData["Year"] = archivaData.Year;
            ViewData["Month"] = archivaData.Month;
            ViewData["Day"] = archivaData.Day;
            ViewData["Posts"] = posts;
            ViewData["PostCounts"] = postCounts;
            ViewData["Months"] = area != null
                                     ? PostRepository.GetArchiveList(area)
                                     : PostRepository.GetArchiveList(Config.Site.ID);

            return View();
        }
    }
}