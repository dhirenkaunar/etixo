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
    public class PageController : BaseController
    {
        private const string antiForgeryTokenSalt = "Oxite gots pages!!!1";
        private static string[] pageEditorRoles = new string[] {"SiteOwner", "AreaOwner"};

        public PageController()
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
            TagRepository = dataProvider.TagRepository;
        }

        public PageController(RouteCollection routes, IOxiteConfiguration config, NameValueCollection appSettings,
                              IAreaRepository areaRepository, IMembershipRepository membershipRepository,
                              IPostRepository postRepository, IResourceRepository resourceRepository,
                              ITagRepository tagRepository)
            : base(routes, config, appSettings, areaRepository, membershipRepository, postRepository, resourceRepository
                )
        {
            TagRepository = tagRepository;
        }

        protected ITagRepository TagRepository { get; private set; }

        public virtual ActionResult Index(string pagePath, RequestMode mode)
        {
            string[] pagePathParts = pagePath.Split('/');
            string viewName = "Index";

            if (string.Compare(pagePathParts.Last(), "edit", true) == 0)
            {
                mode = RequestMode.Edit;
            }
            else if (string.Compare(pagePathParts.Last(), "add", true) == 0)
            {
                mode = RequestMode.Add;
            }

            if (mode != RequestMode.View)
            {
                pagePath = string.Join("/", pagePathParts.Take(pagePathParts.Length - 1).ToArray());
            }

            IPost page = pagePath.LoadPage((g, s) => PostRepository.GetPost(g, s));

            if (page == null && mode != RequestMode.Add)
            {
                return NotFound();
            }

            if (mode != RequestMode.View
                &&
                (!User.IsAnonymous &&
                 MembershipRepository.GetUserIsInRoleAny(MembershipRepository.GetUser(User.Username), pageEditorRoles)))
            {
                ViewData["NewParentPostID"] = mode == RequestMode.Add && page != null ? page.ID : Guid.Empty;

                page = mode != RequestMode.Add ? page : PostRepository.CreatePost();
                viewName = "Manage";

                ViewData["Creator"] = page.CreatorUser ?? User;
                ViewData["RequestMode"] = mode;
                ViewData["Pages"] = addRootPage(PostRepository.GetPostsWithFullSlug(Config.Site.ID));
                ViewData["ReturnUri"] = Request.UrlReferrer != null &&
                                        Request.UrlReferrer.AbsolutePath != Request.Url.AbsolutePath &&
                                        mode != RequestMode.Add
                                            ? Request.UrlReferrer.AbsolutePath
                                            : (mode == RequestMode.Edit
                                                   ? Url.RouteUrl("Page", new {pagePath = pagePath})
                                                   : null);
            }

            if (mode == RequestMode.View)
            {
                PageTitle.Post = page;
            }
            else if (mode == RequestMode.Edit)
            {
                PageTitle.AdditionalPageTitleSegments = new string[] {Localize("Edit Page")};
            }
            else
            {
                PageTitle.AdditionalPageTitleSegments = new string[] {Localize("Create Page")};
            }

            ViewData["Post"] = page;
            ViewData["Children"] = PostRepository.GetChildPosts(page.ID);
            ViewData["UrlIsLocked"] = page.State == (byte)EntityState.Normal && page.Published.HasValue &&
                                      ConvertToLocalTime(page.Published.Value).AddHours(Config.Site.PostEditTimeout) <
                                      DateTime.Now;
            ViewData["Meta.Description"] = page.GetBodyShort();
            ViewData["AntiForgeryTicks"] = DateTime.Now.Ticks.ToString();
            ViewData["AntiForgeryToken"] =
                new AntiForgeryToken(this, ViewData["AntiForgeryTicks"] as string).GetHash(antiForgeryTokenSalt);

            return View(viewName);
        }

        [ValidateAntiForgeryToken(Salt = antiForgeryTokenSalt)]
        [AcceptVerbs(HttpVerbs.Post)]
        [OxiteAuthorize(Roles = "SiteOwner,AreaOwner")]
        public virtual ActionResult Index(string pagePath, FormCollection form, Guid? parentID, RequestMode mode)
        {
            string[] pagePathParts = pagePath.Split('/');
            string viewName = "Manage";

            if (string.Compare(pagePathParts.Last(), "edit", true) == 0)
            {
                mode = RequestMode.Edit;
            }
            else if (string.Compare(pagePathParts.Last(), "add", true) == 0)
            {
                mode = RequestMode.Add;
            }
            else if (string.Compare(pagePathParts.Last(), "remove", true) == 0)
            {
                return removePage(string.Join("/", pagePathParts.Take(pagePathParts.Length - 1).ToArray()),
                                  form["returnUri"]);
            }
            else
            {
                return NotFound();
            }

            pagePath = string.Join("/", pagePathParts.Take(pagePathParts.Length - 1).ToArray());

            IPost page = mode != RequestMode.Add ? pagePath.LoadPage((g, s) => PostRepository.GetPost(g, s)) : null;
            IPost newParent = PostRepository.GetPost(parentID ?? Guid.Empty) ??
                              pagePath.LoadPage((g, s) => PostRepository.GetPost(g, s));
            bool urlIsLocked = page != null && page.State == (byte)EntityState.Normal && page.Published.HasValue &&
                               ConvertToLocalTime(page.Published.Value).AddHours(Config.Site.PostEditTimeout) <
                               DateTime.Now;

            if (mode == RequestMode.Edit)
            {
                PageTitle.AdditionalPageTitleSegments = new string[] {Localize("Edit Page")};
            }
            else
            {
                PageTitle.AdditionalPageTitleSegments = new string[] {Localize("Create Page")};
            }

            List<ITag> tags =
                form.LoadTags((s) => TagRepository.GetTag(s), (s) => TagRepository.CreateTag(s), ModelState) as
                List<ITag>;
            IPost pageFromRequest = form.LoadPost(() => PostRepository.CreatePost(), ModelState);

            if (!urlIsLocked && isDuplicateUri(page, pageFromRequest.Slug, newParent))
            {
                ModelState.Add("Post.Slug", new ModelState() {AttemptedValue = pageFromRequest.Slug});
                ModelState["Post.Slug"].Errors.Add(new ModelError(Localize("This slug is already being used!")));
            }

            if (pageFromRequest != null && ModelState.IsValid)
            {
                if (page == null)
                {
                    page = PostRepository.CreatePost();
                    page.CreatorUserID = User.ID;
                }

                page.Slug = pageFromRequest.Slug;
                page.Title = pageFromRequest.Title;
                page.Body = pageFromRequest.Body;
                page.BodyShort = string.Empty;
                page.State = pageFromRequest.State;
                page.Published = pageFromRequest.Published.HasValue
                                     ? ConvertFromLocalTime(pageFromRequest.Published.Value)
                                     : (DateTime?)null;
                page.Modified = pageFromRequest.Modified;

                if (page.ID == Guid.Empty)
                {
                    page.Created = DateTime.Now.ToUniversalTime();

                    ISubscription subscription = null;

                    if (Config.Site.AuthorAutoSubscribe)
                    {
                        subscription = PostRepository.CreateSubscription();
                        subscription.UserID = User.ID;
                    }

                    PostRepository.AddPost(page, subscription);
                }

                IPost currentParent = PostRepository.GetParentPost(page.ID);
                if (currentParent == null || newParent == null || currentParent.ID != newParent.ID)
                {
                    if (currentParent != null)
                    {
                        PostRepository.RemovePostRelationship(page.ID);
                    }

                    PostRepository.AddPostRelationship(Config.Site.ID, page.ID,
                                                       newParent != null ? newParent.ID : Guid.Empty);
                }

                foreach (ITag tag in tags)
                {
                    if (tag.ID == Guid.Empty)
                    {
                        TagRepository.AddTag(tag);
                    }
                    PostRepository.AddPostToTag(page.ID, tag.ID);
                }

                page.Tags.ToList().FindAll(tP => !tags.Exists(t => t.ID == tP.ID)).ForEach(
                    t => PostRepository.RemovePostFromTag(page.ID, t.ID));

                PostRepository.SubmitChanges();

                if (!string.IsNullOrEmpty(form["returnUri"]))
                {
                    return new RedirectResult(form["returnUri"]);
                }
                else
                {
                    return Redirect(page.GetPageUrl(ControllerContext, Routes));
                }
            }

            ViewData["Post"] = page ?? PostRepository.CreatePost();
            ViewData["NewParentPostID"] = newParent != null ? newParent.ID : Guid.Empty;
            ViewData["Pages"] = addRootPage(PostRepository.GetPostsWithFullSlug(Config.Site.ID));
            ViewData["Creator"] = page != null ? page.CreatorUser : User;
            ViewData["RequestMode"] = mode;
            ViewData["UrlIsLocked"] = urlIsLocked;
            ViewData["ReturnUri"] = form["returnUri"];
            ViewData["Meta.Description"] = page != null ? page.GetBodyShort() : string.Empty;
            ViewData["AntiForgeryTicks"] = DateTime.Now.Ticks.ToString();
            ViewData["AntiForgeryToken"] =
                new AntiForgeryToken(this, ViewData["AntiForgeryTicks"] as string).GetHash(antiForgeryTokenSalt);

            return View(viewName);
        }

        private bool isValidPagePath(string pagePath, out IPost post)
        {
            string[] pagePathParts = pagePath.Split('/');

            post = pagePath.LoadPage((g, s) => PostRepository.GetPost(g, s));

            return post != null && (post.State == (byte)EntityState.Normal || !User.IsAnonymous);
        }

        private static Dictionary<Guid, string> addRootPage(Dictionary<Guid, string> pages)
        {
            Dictionary<Guid, string> newPages = new Dictionary<Guid, string>(pages.Count + 1);

            newPages.Add(Guid.Empty, "/");
            foreach (KeyValuePair<Guid, string> page in pages)
            {
                newPages.Add(page.Key, page.Value);
            }

            return newPages;
        }

        private bool isDuplicateUri(IPost post, string slug, IPost parentPost)
        {
            IPost candidate = PostRepository.GetPost(parentPost != null ? parentPost.ID : Guid.Empty, slug);
            return candidate != null && candidate.ID != (post != null ? post.ID : Guid.Empty) &&
                   (candidate.Parent.ID == (parentPost != null ? parentPost.ID : Guid.Empty) ||
                    (parentPost == null && candidate.ID == candidate.Parent.ID));
        }

        [OxiteAuthorize(Roles = "SiteOwner,AreaOwner")]
        private ActionResult removePage(string pagePath, string returnUri)
        {
            IPost post = pagePath.LoadPage((g, s) => PostRepository.GetPost(g, s));
            IQueryable<IPost> children = PostRepository.GetChildPosts(post.ID);

            if (children.Count() > 0)
            {
                //todo(nheskew): use the errors for something
                ModelState.Add("Page.Remove", new ModelState() {AttemptedValue = pagePath});
                ModelState["Page.Remove"].Errors.Add(
                    new ModelError("Cannot remove this page because it has other, child, pages tied to it."));

                return Redirect(returnUri);
            }

            if (post != null)
            {
                PostRepository.RemovePost(post.ID);
                PostRepository.SubmitChanges();
            }

            if (!string.IsNullOrEmpty(returnUri))
            {
                return Redirect(returnUri);
            }
            else
            {
                return Json(new {removed = post != null ? post.ID.ToString() : false.ToString()});
            }
        }
    }
}