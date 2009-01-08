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
    public class TagController : BaseController
    {
        public TagController()
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
            TagRepository = Config.DataProvider.GetInstance().TagRepository;
        }

        public TagController(RouteCollection routes, IOxiteConfiguration config, NameValueCollection appSettings,
                             IAreaRepository areaRepository, IMembershipRepository membershipRepository,
                             IPostRepository postRepository, IResourceRepository resourceRepository,
                             ITagRepository tagRepository)
            : base(routes, config, appSettings, areaRepository, membershipRepository, postRepository, resourceRepository
                )
        {
            TagRepository = tagRepository;
        }

        protected ITagRepository TagRepository { get; set; }

        public virtual ActionResult Index()
        {
            PageTitle.AdditionalPageTitleSegments = new string[] {"Tags"};

            ViewData["Tags"] = TagRepository.GetTagsWithPostCount();
            ViewData["Months"] = PostRepository.GetArchiveList(Config.Site.ID);

            return View();
        }

        public virtual ActionResult Permalink(string tagName, int page)
        {
            ITag tag = TagRepository.GetTag(tagName);
            IPageOfAList<IPost> posts = PostRepository.GetPosts(Config.Site.ID, tag, page - 1, 10);
            Dictionary<Guid, int> postCounts = new Dictionary<Guid, int>(posts.Count);

            PageTitle.AdditionalPageTitleSegments = new string[] {"Tags", tag.Name};

            posts.ToList().ForEach(p => postCounts.Add(p.ID, PostRepository.GetComments(p).Count()));

            ViewData["AreaCount"] = AreaRepository.GetAreasCount(Config.Site.ID);
            ViewData["Tag"] = tag;
            ViewData["Months"] = PostRepository.GetArchiveList(Config.Site.ID);
            ViewData["Posts"] = posts;
            ViewData["PostCounts"] = postCounts;

            AddPermalinkFeedDiscovery(tag);

            return View();
        }

        public virtual ActionResult PermalinkAtom(string tagName)
        {
            return PermalinkFeed(tagName);
        }

        public virtual ActionResult PermalinkRss(string tagName)
        {
            return PermalinkFeed(tagName);
        }

        protected virtual ActionResult PermalinkFeed(string tagName)
        {
            ITag tag = TagRepository.GetTag(tagName);

            PageTitle.AdditionalPageTitleSegments = new string[] {"Tags", tag.Name};

            ViewData["Tag"] = tag;

            return Feed(PostRepository.GetPosts(Config.Site.ID, tag, 0, 50).Cast<IFeedItem>());
        }

        public FeedResult AllCommentsRss(string tagName)
        {
            return AllCommentsFeed(tagName);
        }

        public FeedResult AllCommentsAtom(string tagName)
        {
            return AllCommentsFeed(tagName);
        }

        public FeedResult AllCommentsFeed(string tagName)
        {
            ITag tag = TagRepository.GetTag(tagName);

            PageTitle.AdditionalPageTitleSegments = new string[] {"Tags", tag.Name, "All Comments"};

            ViewData["ChannelLink"] = GetAbsolutePath(Url.RouteUrl("TagPermalink", new {tagName = tag.Name}));

            return Feed(PostRepository.GetComments(tag, 0, 50).Cast<IFeedItem>());
        }

        protected virtual void AddPermalinkFeedDiscovery(ITag tag)
        {
            RegisterFeed(string.Format("All {0} Comments", tag.Name),
                         f => Url.RouteUrl(string.Format(f, "TagPermalinkAllComments"), new {tagName = tag.Name}));
            RegisterFeed(string.Format("{0} Posts", tag.Name),
                         f => Url.RouteUrl(string.Format(f, "TagPermalink"), new {tagName = tag.Name}));
        }
    }
}