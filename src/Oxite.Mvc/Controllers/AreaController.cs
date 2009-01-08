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
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Oxite.Configuration;
using Oxite.Data;

namespace Oxite.Mvc.Controllers
{
    public class AreaController : BaseController
    {
        private const string antiForgeryTokenSalt = "I can has Oxite?";

        public AreaController()
        {
            Routes = RouteTable.Routes;
            Config = (IOxiteConfiguration)ConfigurationManager.GetSection("oxite");
            AppSettings = new AppSettingsHelper(ConfigurationManager.AppSettings);
            PageTitle = new PageTitleHelper(Config);

            IOxiteDataProvider dataProvider = Config.DataProvider.GetInstance();
            AreaRepository = dataProvider.AreaRepository;
            LanguageRepository = dataProvider.LanguageRepository;
            MembershipRepository = dataProvider.MembershipRepository;
            PostRepository = dataProvider.PostRepository;
            ResourceRepository = dataProvider.ResourceRepository;
            PostRepository.CommentAdding +=
                new EventHandler<OxiteCancelDataItemEventArgs<IComment>>(PostRepository_CommentAdding);
        }

        public AreaController(RouteCollection routes, IOxiteConfiguration config, NameValueCollection appSettings,
                              IAreaRepository areaRepository, IMembershipRepository membershipRepository,
                              IPostRepository postRepository, ILanguageRepository languageRepository,
                              IResourceRepository resourceRepository)
            : base(routes, config, appSettings, areaRepository, membershipRepository, postRepository, resourceRepository
                )
        {
            LanguageRepository = languageRepository;
        }

        protected ILanguageRepository LanguageRepository { get; set; }

        public virtual ActionResult Index(string areaName, int page)
        {
            IArea area = AreaRepository.GetArea(areaName);
            IPageOfAList<IPost> posts = PostRepository.GetPosts(area, page - 1, 10);
            Dictionary<Guid, int> postCounts = new Dictionary<Guid, int>(posts.Count);

            PageTitle.Area = area;

            posts.ToList().ForEach(p => postCounts.Add(p.ID, PostRepository.GetComments(p).Count()));

            ViewData["Area"] = area;
            ViewData["Posts"] = posts;
            ViewData["PostCounts"] = postCounts;
            ViewData["Months"] = AreaRepository.GetArchiveList(area);
            ViewData["RsdLink"] = GenerateRsdLink(area);

            AddIndexFeedDiscovery(area);

            return View();
        }

        public virtual FeedResult IndexAtom(string areaName)
        {
            return IndexFeed(areaName);
        }

        public virtual FeedResult IndexRss(string areaName)
        {
            return IndexFeed(areaName);
        }

        protected virtual FeedResult IndexFeed(string areaName)
        {
            IArea area = AreaRepository.GetArea(areaName);

            if (AreaRepository.GetAreasCount(Config.Site.ID) > 1)
            {
                PageTitle.Area = area;
            }

            ViewData["Area"] = area;

            return Feed(PostRepository.GetPosts(area, 0, 50).Cast<IFeedItem>());
        }

        [PingbackDiscovery]
        [TrackbackDiscovery]
        public virtual ActionResult Permalink(string areaName, string slug, EntityState? commentState)
        {
            IArea area = AreaRepository.GetArea(areaName);
            IPost post = PostRepository.GetPost(area, slug);

            if (post == null)
            {
                return NotFound();
            }
            if (AreaRepository.GetAreas(Config.Site.ID).Count() > 1)
            {
                PageTitle.Area = area;
            }
            else
            {
                PageTitle.Area = null;
            }

            PageTitle.Post = post;

            ViewData["Area"] = area;
            ViewData["Post"] = post;
            ViewData["Comments"] = PostRepository.GetComments(post);
            ViewData["Creator"] = post.CreatorUser;
            ViewData["Meta.Description"] = post.GetBodyShort();
            ViewData["CommentState"] = commentState;
            ViewData["CommentAnonymous"] =
                Request.Cookies["anon"].ToCommentAnonymous(() => PostRepository.GetCommentAnonymousType());
            ViewData["UserLanguages"] = LanguageRepository.GetLanguages(User);
            ViewData["DefaultLanguage"] = User.IsAnonymous
                                              ? LanguageRepository.GetLanguage(Config.Site.LanguageDefault)
                                              : User.LanguageDefault;
            ViewData["AntiForgeryTicks"] = DateTime.Now.Ticks.ToString();
            ViewData["AntiForgeryToken"] =
                new AntiForgeryToken(this, ViewData["AntiForgeryTicks"] as string).GetHash(antiForgeryTokenSalt);

            AddPermalinkFeedDiscovery(area, post);

            return View();
        }

        [ValidateAntiForgeryToken(Salt = antiForgeryTokenSalt)]
        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Permalink(string areaName, string slug, FormCollection form)
        {
            IArea area = AreaRepository.GetArea(areaName);
            IPost post = PostRepository.GetPost(area, slug);
            ICommentAnonymous commentAnonymous = null;
            ISubscription subscription = null;
            ISubscriptionAnonymous subscriptionAnonymous = null;
            string commentBody = "";
            bool subscribe = false;

            if (post == null)
            {
                return NotFound();
            }

            commentBody = form.LoadCommentBody(ModelState);
            subscribe = form.IsTrue("comment_subscribe");

            if (User.IsAnonymous)
            {
                commentAnonymous = form.LoadCommentAnonymous(() => PostRepository.CreateCommentAnonymous(), ModelState);

                subscriptionAnonymous =
                    form.LoadSubscriptionAnonymous(() => PostRepository.CreateSubscriptionAnonymous(), ModelState);
            }

            if (ViewData.ModelState.IsValid)
            {
                IComment comment = PostRepository.CreateComment();

                comment.PostID = post.ID;
                comment.Body = commentBody;
                comment.CreatorUserID = User.ID;
                comment.LanguageID = LanguageRepository.GetLanguage(Config.Site.LanguageDefault).ID;
                    //TODO: (erikpo) Add code to get the selected language from a dropdown list in UI
                comment.CreatorIP = Request.GetUserIPAddress().ToLong();
                comment.UserAgent = Request.UserAgent;
                comment.State = User.IsAnonymous
                                    ? (byte)
                                      ((EntityState)
                                       Enum.Parse(typeof (EntityState), Config.Site.CommentAnonymousStateDefault))
                                    : (byte)EntityState.Normal;

                if (commentAnonymous != null)
                {
                    if (form.IsTrue("comment_remember"))
                    {
                        Response.Cookies.Add(new HttpCookie("anon",
                                                            commentAnonymous.ToJson(
                                                                () => PostRepository.GetCommentAnonymousType()))
                                             {Expires = DateTime.Now.AddDays(14)});
                    }
                    else if (Request.Cookies["anon"] != null)
                    {
                        Response.Cookies.Add(new HttpCookie("anon", string.Empty) {Expires = DateTime.Now.AddDays(-1)});
                    }
                }

                if (subscribe)
                {
                    subscription = PostRepository.CreateSubscription();

                    subscription.UserID = User.ID;
                }

                PostRepository.AddComment(comment, commentAnonymous, subscription, subscriptionAnonymous);

                PostRepository.SubmitChanges();

                if (comment.ID == Guid.Empty)
                {
                    ModelState.AddModelError("Comment", Localize("Unable to add comment."));
                }

                if (comment.Published.HasValue)
                {
                    return Redirect(comment.GetUrl(ControllerContext, Routes));
                }
                else
                {
                    return RedirectToRoute(string.Format("{0}CommentPendingPermalink", area.Type),
                                           new {areaName = areaName, slug = slug});
                }
            }

            ViewData["Area"] = area;
            ViewData["Post"] = post;
            ViewData["Creator"] = post.CreatorUser;
            ViewData["Comments"] = PostRepository.GetComments(post);
            ViewData["Meta.Description"] = post.GetBodyShort();
            ViewData["AntiForgeryTicks"] = DateTime.Now.Ticks.ToString();
            ViewData["AntiForgeryToken"] =
                new AntiForgeryToken(this, ViewData["AntiForgeryTicks"] as string).GetHash(antiForgeryTokenSalt);
            ViewData["CommentAnonymous"] = commentAnonymous;

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ContentResult ComputeHash(string value)
        {
            return Content(value.ComputeHash(), "text/plain");
        }

        public virtual ActionResult PermalinkAtom(string areaName, string slug)
        {
            return PermalinkFeed(areaName, slug);
        }

        public virtual ActionResult PermalinkRss(string areaName, string slug)
        {
            return PermalinkFeed(areaName, slug);
        }

        protected virtual ActionResult PermalinkFeed(string areaName, string slug)
        {
            IArea area = AreaRepository.GetArea(areaName);
            IPost post = PostRepository.GetPost(area, slug);

            if (post == null)
            {
                return NotFound();
            }

            if (AreaRepository.GetAreasCount(Config.Site.ID) > 1)
            {
                PageTitle.Area = area;
            }

            PageTitle.Post = post;

            ViewData["Area"] = area;
            ViewData["Post"] = post;

            return Feed(PostRepository.GetComments(post).Cast<IFeedItem>());
        }

        public FeedResult AllCommentsRss(string areaName)
        {
            return AllCommentsFeed(areaName);
        }

        public FeedResult AllCommentsAtom(string areaName)
        {
            return AllCommentsFeed(areaName);
        }

        public FeedResult AllCommentsFeed(string areaName)
        {
            IArea area = AreaRepository.GetArea(areaName);

            PageTitle.Area = area;
            PageTitle.AdditionalPageTitleSegments = new string[] {Localize("All Comments")};

            ViewData["ChannelLink"] = GetAbsolutePath(area.GetUrl(ControllerContext, Routes));

            return Feed(PostRepository.GetComments(area, 0, 50).Cast<IFeedItem>());
        }

        public virtual ActionResult Rsd(string areaName)
        {
            return Xml(GenerateRsd(areaName));
        }

        protected virtual void AddIndexFeedDiscovery(IArea area)
        {
            if (AreaRepository.GetAreasCount(Config.Site.ID) > 1)
            {
                RegisterFeed(string.Format(Localize("All {0} Comments"), area.Name),
                             f =>
                             Url.RouteUrl(string.Format(f, string.Format("{0}AllComments", area.Type)),
                                          new {areaName = area.Name}));
                RegisterFeed(area.DisplayName, f => area.GetUrl(f, ControllerContext, Routes));
            }
        }

        protected virtual void AddPermalinkFeedDiscovery(IArea area, IPost post)
        {
            AddIndexFeedDiscovery(area);
            RegisterFeed(string.Format(Localize("{0} Comments"), post.Title),
                         f => post.GetAreaPostUrl(f, ControllerContext, Routes));
        }

        private void PostRepository_CommentAdding(object sender, OxiteCancelDataItemEventArgs<IComment> e)
        {
            OnCommentAdding(e);
        }

        protected virtual void OnCommentAdding(OxiteCancelDataItemEventArgs<IComment> e)
        {
            e.Item.Body = e.Item.Body.CleanHtml();
        }
    }
}