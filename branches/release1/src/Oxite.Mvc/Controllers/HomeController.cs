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
    [HandleError]
    public class HomeController : BaseController
    {
        public HomeController()
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

        public HomeController(RouteCollection routes, IOxiteConfiguration config, NameValueCollection appSettings,
                              IAreaRepository areaRepository, IMembershipRepository membershipRepository,
                              IPostRepository postRepository, IResourceRepository resourceRepository)
            : base(routes, config, appSettings, areaRepository, membershipRepository, postRepository, resourceRepository
                )
        {
        }

        public virtual ActionResult Index(int page)
        {
            IPageOfAList<IPost> posts = PostRepository.GetPosts(Config.Site.ID, page - 1, 10);
            Dictionary<Guid, int> postCounts = new Dictionary<Guid, int>(posts.Count);

            posts.ToList().ForEach(p => postCounts.Add(p.ID, PostRepository.GetComments(p).Count()));

            ViewData["AreaCount"] = AreaRepository.GetAreasCount(Config.Site.ID);
            ViewData["Posts"] = posts;
            ViewData["PostCounts"] = postCounts;
            ViewData["Months"] = PostRepository.GetArchiveList(Config.Site.ID);
            ViewData["RsdLink"] = GenerateRsdLink();

            AddIndexFeedDiscovery();

            return View();
        }

        public virtual FeedResult IndexAtom()
        {
            return IndexFeed();
        }

        public virtual FeedResult IndexRss()
        {
            return IndexFeed();
        }

        protected virtual FeedResult IndexFeed()
        {
            ViewData["AreaCount"] = AreaRepository.GetAreasCount(Config.Site.ID);

            return Feed(PostRepository.GetPosts(Config.Site.ID, 0, 50).Cast<IFeedItem>());
        }

        public virtual FeedResult AllCommentsRss()
        {
            return AllCommentsFeed();
        }

        public virtual FeedResult AllCommentsAtom()
        {
            return AllCommentsFeed();
        }

        public virtual FeedResult AllCommentsFeed()
        {
            PageTitle.AdditionalPageTitleSegments = new string[] {"All Comments"};

            ViewData["ChannelLink"] = GetAbsolutePath(Url.RouteUrl("Home"));

            return Feed(PostRepository.GetComments(Config.Site.ID, 0, 50).Cast<IFeedItem>());
        }

        public virtual ActionResult Rsd()
        {
            return Xml(GenerateRsd());
        }

        protected virtual void AddIndexFeedDiscovery()
        {
            RegisterFeed("All Comments", f => Url.RouteUrl(string.Format(f, "AllComments")), 0);
        }
    }
}