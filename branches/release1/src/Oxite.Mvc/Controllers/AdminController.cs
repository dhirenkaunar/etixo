//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using BlogML.Xml;
using Oxite.Configuration;
using Oxite.Data;

namespace Oxite.Mvc.Controllers
{
    [OxiteAuthorize(Roles = "SiteOwner,AreaOwner")]
    public class AdminController : BaseController
    {
        private const string antiForgeryTokenSalt = "Oxite FTW!!!1";

        public AdminController()
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
            LanguageRepository = dataProvider.LanguageRepository;
        }

        public AdminController(RouteCollection routes, IOxiteConfiguration config, NameValueCollection appSettings,
                               IAreaRepository areaRepository, IMembershipRepository membershipRepository,
                               IPostRepository postRepository, IResourceRepository resourceRepository,
                               ITagRepository tagRepository, ILanguageRepository languageRepository)
            : base(routes, config, appSettings, areaRepository, membershipRepository, postRepository, resourceRepository
                )
        {
            TagRepository = tagRepository;
            LanguageRepository = languageRepository;
        }

        protected ITagRepository TagRepository { get; private set; }
        protected ILanguageRepository LanguageRepository { get; set; }

        public ActionResult Index(string areaFilter, string publishedFilter, int page)
        {
            PublishedFilter pFilter = !string.IsNullOrEmpty(publishedFilter)
                                          ? (PublishedFilter)Enum.Parse(typeof (PublishedFilter), publishedFilter, true)
                                          : PublishedFilter.All;
            IPageOfAList<IPost> posts;
            Dictionary<Guid, int> postCounts;

            PageTitle.AdditionalPageTitleSegments = new string[] {"Admin"};

            if (areaFilter == null || areaFilter == "all")
            {
                posts = PostRepository.GetPosts(Config.Site.ID, pFilter, page - 1, 10);
            }
            else
            {
                IArea area = AreaRepository.GetArea(areaFilter);

                posts = PostRepository.GetPosts(area, pFilter, page - 1, 10);
            }

            postCounts = new Dictionary<Guid, int>(posts.Count);
            posts.ToList().ForEach(p => postCounts.Add(p.ID, PostRepository.GetComments(p).Count()));

            ViewData["AreaCount"] = AreaRepository.GetAreasCount(Config.Site.ID);
            ViewData["Posts"] = posts;
            ViewData["PostCounts"] = postCounts;

            return View();
        }

        public virtual ActionResult ViewPost(string areaName, string slugToEdit, RequestMode mode)
        {
            IArea area = AreaRepository.GetArea(areaName);
            IPost post = PostRepository.GetPost(area, slugToEdit) ?? PostRepository.CreatePost();
            string viewName = mode != RequestMode.View ? "EditPost" : "ViewPost";
            ;

            if (mode == RequestMode.Add)
            {
                PageTitle.AdditionalPageTitleSegments = new string[] {"Admin", "Create Post"};
            }
            else if (mode == RequestMode.Edit)
            {
                PageTitle.AdditionalPageTitleSegments = new string[] {"Admin", "Edit Post"};
            }

            ViewData["Post"] = post;
            ViewData["Area"] = area;
            ViewData["Areas"] = AreaRepository.GetAreas(Config.Site.ID);
            ViewData["Comments"] = PostRepository.GetComments(post);
            ViewData["Creator"] = post.CreatorUser ?? User;
            ViewData["RequestMode"] = mode;
            ViewData["UrlIsLocked"] = post.State == (byte)EntityState.Normal && post.Published.HasValue &&
                                      ConvertToLocalTime(post.Published.Value).AddHours(Config.Site.PostEditTimeout) <
                                      DateTime.Now;
            ViewData["ReturnUri"] = Request.UrlReferrer != null &&
                                    Request.UrlReferrer.AbsolutePath != Request.Url.AbsolutePath
                                        ? Request.UrlReferrer.AbsolutePath
                                        : (mode == RequestMode.Edit
                                               ? Url.RouteUrl("BlogPermalink",
                                                              new {areaName = areaName, slug = slugToEdit})
                                               : null);
            ViewData["Meta.Description"] = post.GetBodyShort();
            ViewData["AntiForgeryTicks"] = DateTime.Now.Ticks.ToString();
            ViewData["AntiForgeryToken"] =
                new AntiForgeryToken(this, ViewData["AntiForgeryTicks"] as string).GetHash(antiForgeryTokenSalt);

            return View(viewName);
        }

        [ValidateAntiForgeryToken(Salt = antiForgeryTokenSalt)]
        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult ViewPost(string areaName, string slugToEdit, FormCollection form, Guid areaID,
                                             RequestMode mode)
        {
            IArea area = AreaRepository.GetArea(areaName);
            IPost post = PostRepository.GetPost(area, slugToEdit);
            bool urlIsLocked = post != null && post.State == (byte)EntityState.Normal && post.Published.HasValue &&
                               ConvertToLocalTime(post.Published.Value).AddHours(Config.Site.PostEditTimeout) <
                               DateTime.Now;
            bool urlHasChanged = true;

            PageTitle.Area = area;
            PageTitle.Post = post;

            List<ITag> tags =
                form.LoadTags((s) => TagRepository.GetTag(s), (s) => TagRepository.CreateTag(s), ModelState) as
                List<ITag>;
            IPost postFromRequest = form.LoadPost(() => PostRepository.CreatePost(), ModelState);

            if (!urlIsLocked && isDuplicateUri(post, postFromRequest.Slug, AreaRepository.GetArea(areaID)))
            {
                ModelState.Add("Post.Slug", new ModelState() {AttemptedValue = postFromRequest.Slug});
                ModelState["Post.Slug"].Errors.Add(new ModelError(Localize("This slug is already being used!")));
            }

            if (postFromRequest != null && ModelState.IsValid)
            {
                area = AreaRepository.GetArea(areaID);

                if (post == null)
                {
                    post = PostRepository.CreatePost();
                    post.CreatorUserID = User.ID;
                }
                else
                {
                    urlHasChanged =
                        !(string.Compare(post.Area.Name, area.Name, true) == 0 &&
                          string.Compare(post.Slug, postFromRequest.Slug, true) == 0);
                }

                post.Slug = postFromRequest.Slug;
                post.Title = postFromRequest.Title;
                post.Body = postFromRequest.Body;
                post.BodyShort = postFromRequest.BodyShort;
                post.State = postFromRequest.State;
                post.Published = postFromRequest.Published.HasValue
                                     ? ConvertFromLocalTime(postFromRequest.Published.Value)
                                     : (DateTime?)null;
                post.Modified = postFromRequest.Modified;

                if (post.ID == Guid.Empty)
                {
                    post.Created = DateTime.Now.ToUniversalTime();

                    ISubscription subscription = null;

                    if (Config.Site.AuthorAutoSubscribe)
                    {
                        subscription = PostRepository.CreateSubscription();

                        subscription.UserID = User.ID;
                    }

                    PostRepository.AddPost(post, subscription);
                }

                if (post.Area == null)
                {
                    AreaRepository.AddPostToArea(post.ID, area.ID);
                }
                else if (post.Area.ID != area.ID)
                {
                    AreaRepository.RemovePostFromArea(post.ID, post.Area.ID);
                    AreaRepository.AddPostToArea(post.ID, area.ID);
                }

                if (PostRepository.GetParentPost(post.ID) != null)
                {
                    PostRepository.RemovePostRelationship(post.ID);
                }

                foreach (ITag tag in tags)
                {
                    if (tag.ID == Guid.Empty)
                    {
                        TagRepository.AddTag(tag);
                    }
                    PostRepository.AddPostToTag(post.ID, tag.ID);
                }

                post.Tags.ToList().FindAll(tP => !tags.Exists(t => t.ID == tP.ID)).ForEach(
                    t => PostRepository.RemovePostFromTag(post.ID, t.ID));

                PostRepository.SubmitChanges();

                if (!string.IsNullOrEmpty(form["returnUri"]) && !urlHasChanged)
                {
                    return new RedirectResult(form["returnUri"]);
                }
                else
                {
                    return RedirectToRoute(
                        "BlogPermalink",
                        new
                        {
                            areaName = area.Name,
                            slug = post.Slug
                        }
                        );
                }
            }

            ViewData["Post"] = post ?? PostRepository.CreatePost();
            ViewData["Area"] = area;
            ViewData["Areas"] = AreaRepository.GetAreas(Config.Site.ID);
            ViewData["Comments"] = PostRepository.GetComments(post);
            ViewData["Creator"] = post != null ? post.CreatorUser : User;
            ViewData["RequestMode"] = mode;
            ViewData["UrlIsLocked"] = urlIsLocked;
            ViewData["ReturnUri"] = form["returnUri"];
            ViewData["Meta.Description"] = post != null ? post.GetBodyShort() : string.Empty;
            ViewData["AntiForgeryTicks"] = DateTime.Now.Ticks.ToString();
            ViewData["AntiForgeryToken"] =
                new AntiForgeryToken(this, ViewData["AntiForgeryTicks"] as string).GetHash(antiForgeryTokenSalt);

            return View("EditPost");
        }

        private bool isDuplicateUri(IPost post, string slug, IArea area)
        {
            IPost candidate = PostRepository.GetPost(area, slug);
            return candidate != null && candidate.ID != (post != null ? post.ID : Guid.Empty);
        }

        public virtual ActionResult TagPermalink(string tagName, int page)
        {
            ITag tag = TagRepository.GetTag(tagName);
            IPageOfAList<IPost> posts = PostRepository.GetPosts(Config.Site.ID, tag, page - 1, 10);
            Dictionary<Guid, int> postCounts = new Dictionary<Guid, int>(posts.Count);

            PageTitle.AdditionalPageTitleSegments = new string[] {"Tags", tag.Name};

            posts.ToList().ForEach(p => postCounts.Add(p.ID, PostRepository.GetComments(p).Count()));

            ViewData["Tag"] = tag;
            ViewData["Posts"] = posts;
            ViewData["PostCounts"] = postCounts;
            ViewData["AreaCount"] = AreaRepository.GetAreasCount(Config.Site.ID);

            return View();
        }

        public virtual ActionResult Files()
        {
            ViewData["Files"] = ResourceRepository.GetFiles(Config.Site.ID, User.ID);

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Files(string path)
        {
            HttpPostedFileBase postedFile = Request.Files[0];
            byte[] bytes = new byte[postedFile.ContentLength];

            postedFile.InputStream.Read(bytes, 0, postedFile.ContentLength);

            IFileResource fileResource = ResourceRepository.CreateFile();
            string filePath = Path.GetFileName(postedFile.FileName);

            if (!filePath.StartsWith("/"))
            {
                filePath = "/" + filePath;
            }
            if (!string.IsNullOrEmpty(path))
            {
                filePath = path + filePath;
            }
            if (filePath.StartsWith("/"))
            {
                filePath = filePath.Substring(1);
            }

            fileResource.SiteID = Config.Site.ID;
            fileResource.CreatorUser = User;
            fileResource.ContentType = postedFile.ContentType;
            fileResource.Content = bytes;
            fileResource.Path = "";
            fileResource.Name = filePath;
            fileResource.State = (byte)EntityState.Normal;

            ResourceRepository.AddFile(fileResource);
            ResourceRepository.AddFileToUser(fileResource.ID, User.ID);
            ResourceRepository.SubmitChanges();

            return RedirectToAction("Files");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult FileModify(string type, Guid fileID)
        {
            IFileResource fileResource = ResourceRepository.GetFile(Config.Site.ID, User.ID, fileID);

            if (fileResource == null)
            {
                throw new Exception("Could not find file to remove");
            }

            if (string.Compare(type, "remove", true) == 0)
            {
                ResourceRepository.RemoveFileFromUser(fileResource.ID, User.ID);
                ResourceRepository.RemoveFile(fileResource.ID);
                ResourceRepository.SubmitChanges();
            }
            else
            {
                throw new ArgumentException("Invalid file modification type specified", "type");
            }

            return RedirectToAction("Files");
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public virtual ActionResult Areas(string areaNameSearch)
        {
            PageTitle.AdditionalPageTitleSegments = new string[] {"Admin", "Areas"};

            if (!string.IsNullOrEmpty(areaNameSearch))
            {
                ViewData["AreaNameSearch"] = areaNameSearch;
                ViewData["Areas"] = AreaRepository.FindAreas(Config.Site.ID, areaNameSearch);
            }

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public virtual ActionResult AreasEdit(Guid areaID, string areaName, string areaDisplayName,
                                              string areaDescription, string areaType)
        {
            List<string> titleSegments = new List<string>();

            titleSegments.Add("Admin");
            titleSegments.Add("Areas");

            if (areaID == Guid.Empty)
            {
                if (string.Compare(Request.HttpMethod, HttpVerbs.Post.ToString(), true) == 0)
                {
                    IArea existingArea = AreaRepository.GetArea(areaName);

                    titleSegments.Add("Add");
                    PageTitle.AdditionalPageTitleSegments = titleSegments.ToArray();

                    if (existingArea != null)
                    {
                        ViewData["AreaName"] = areaName;
                        ViewData["AreaDisplayName"] = areaDisplayName;
                        ViewData["AreaDescription"] = areaDescription;
                        ViewData["AreaType"] = areaType;
                        ViewData["Message"] = string.Format("Area {0} already exists",
                                                            string.Format("<a href=\"{1}\">{0}</a>", existingArea.Name,
                                                                          existingArea.GetAdminUrl(ControllerContext,
                                                                                                   Routes)));

                        return View();
                    }
                    else
                    {
                        IArea area = AreaRepository.CreateArea();

                        area.SiteID = Config.Site.ID;
                        area.Name = areaName;
                        area.DisplayName = areaDisplayName;
                        area.Description = areaDescription;
                        area.Type = areaType;
                        area.TypeUrl = "";

                        AreaRepository.AddArea(area);
                        AreaRepository.SubmitChanges();

                        ((OxiteApplication)HttpContext.ApplicationInstance).RegisterRoutes();

                        ViewData["Message"] = string.Format(Localize("Area {0} has been created"),
                                                            string.Format("<a href=\"{1}\">{0}</a>", area.Name,
                                                                          area.GetAdminUrl(ControllerContext, Routes)));

                        Areas(null);

                        return View("Areas");
                    }
                }
                else
                {
                    return View();
                }
            }
            else
            {
                IArea area = AreaRepository.GetArea(areaID);

                if (area == null)
                {
                    return NotFound(string.Format(Localize("An Area with an ID of '{0}' could not be found"), areaID));
                }

                if (string.Compare(Request.HttpMethod, HttpVerbs.Get.ToString(), true) == 0)
                {
                    titleSegments.Add("Edit");
                    PageTitle.AdditionalPageTitleSegments = titleSegments.ToArray();

                    ViewData["Area"] = area;

                    return View();
                }
                else
                {
                    area.Name = AreaRepository.CleanAreaName(areaName);
                    area.DisplayName = areaDisplayName;
                    area.Description = areaDescription;
                    area.Type = areaType;

                    AreaRepository.SubmitChanges();

                    ViewData["Message"] = string.Format(Localize("Area {0} has been modified"),
                                                        string.Format("<a href=\"{1}\">{0}</a>", area.Name,
                                                                      area.GetAdminUrl(ControllerContext, Routes)));

                    Areas(null);

                    return View("Areas");
                }
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemovePost(string areaName, string slug, string returnUri)
        {
            IArea area = AreaRepository.GetArea(areaName);
            IPost post = PostRepository.GetPost(area, slug);

            return removePost(post, returnUri);
        }

        private ActionResult removePost(IPost post, string returnUri)
        {
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

        public ActionResult Comments(string areaName, Guid? slug, int page)
        {
            //todo(nheskew): add filters for area, post or tag - and fix up the " - 1, 10" on all the page requests
            ViewData["Comments"] = PostRepository.GetComments(Config.Site.ID, page - 1, 10);

            return View("CommentsOnly");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SetCommentState(Guid commentID, EntityState commentState, string returnUri)
        {
            IComment comment = PostRepository.GetComment(commentID);

            if (comment != null)
            {
                comment.State = (byte)commentState;
                PostRepository.SubmitChanges();
            }

            if (!string.IsNullOrEmpty(returnUri))
            {
                return Redirect(returnUri);
            }
            else
            {
                return Json(new {modifies = comment != null ? comment.ID.ToString() : false.ToString()});
            }
        }

        public ActionResult Settings()
        {
            PageTitle.AdditionalPageTitleSegments = new string[] {"Admin", Localize("Settings")};

            ViewData["Area"] = AreaRepository.GetAllowedAreas(Config.Site.ID, User.ID).First();

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult BlogML(string areaName, string slugPattern)
        {
            IArea area = AreaRepository.GetArea(areaName);
            IUser user = User;
                //TODO: (erikpo) Change this to be user selectable in the multiple blogs case if the user is a site owner

            if (area == null)
            {
                return NotFound();
            }

            if (string.Compare(Request.HttpMethod, HttpVerbs.Post.ToString(), true) == 0)
            {
                //TODO: (erikpo) Need to display error messages instead of not found page
                if (!MembershipRepository.GetUserIsInRole(User.ID, area))
                {
                    return NotFound();
                }

                XmlTextReader reader = null;

                try
                {
                    reader = new XmlTextReader(Request.Files[0].InputStream);
                    BlogMLBlog blog = BlogMLSerializer.Deserialize(reader);

                    Guid defaultLanguageID = LanguageRepository.GetLanguage(Config.Site.LanguageDefault).ID;

                    area.Description = blog.SubTitle;
                    area.DisplayName = blog.Title;
                    area.Created = blog.DateCreated;

                    AreaRepository.SubmitChanges();

                    int postCount = PostRepository.GetPosts(area, 0, 1).TotalItemCount;

                    IPageOfAList<IPost> posts = PostRepository.GetPosts(area, 0, postCount);

                    foreach (IPost item in posts)
                    {
                        PostRepository.RemovePost(item.ID);
                    }

                    PostRepository.SubmitChanges();

                    Dictionary<string, Guid> UserReference = new Dictionary<string, Guid>();

                    UserReference.Add(blog.Authors[0].ID, user.ID);

                    Dictionary<string, Guid> TagReference = new Dictionary<string, Guid>();

                    foreach (BlogMLCategory tag in blog.Categories)
                    {
                        ITag t = TagRepository.CreateTag(tag.Title);

                        t.Created = tag.DateCreated;
                        t.ID = Guid.NewGuid();

                        TagRepository.AddTag(t);
                        TagReference.Add(tag.ID, t.ID);
                    }

                    TagRepository.SubmitChanges();

                    Dictionary<string, Guid> PostReference = new Dictionary<string, Guid>();

                    foreach (BlogMLPost post in blog.Posts)
                    {
                        IPost newPost = PostRepository.CreatePost();

                        newPost.ID = Guid.NewGuid();

                        newPost.Body = post.Content.Text;
                        newPost.CreatorUserID = user.ID;
                        newPost.Modified = post.DateModified;
                        newPost.Published = post.DateCreated;
                        if (!string.IsNullOrEmpty(slugPattern))
                        {
                            Regex regex = new Regex(slugPattern);
                            Match match = regex.Match(post.PostUrl);

                            if (match != null && match.Groups != null && match.Groups.Count >= 2)
                            {
                                newPost.Slug = match.Groups[1].Value;
                            }
                            else
                            {
                                newPost.Slug = post.ID;
                            }
                        }
                        else
                        {
                            newPost.Slug = post.PostUrl;
                        }
                        newPost.Title = post.Title;
                        if (post.HasExcerpt)
                        {
                            newPost.BodyShort = post.Excerpt.Text;
                        }
                        else
                        {
                            newPost.BodyShort = "";
                        }

                        if (post.Approved)
                        {
                            newPost.State = (byte)EntityState.Normal;
                        }
                        else
                        {
                            newPost.State = (byte)EntityState.PendingApproval;
                        }

                        PostRepository.AddPost(newPost, null);
                        PostRepository.SubmitChanges();

                        AreaRepository.AddPostToArea(newPost.ID, area.ID);
                        AreaRepository.SubmitChanges();

                        PostReference.Add(post.ID, newPost.ID);

                        foreach (BlogMLCategoryReference bcr in post.Categories)
                        {
                            Guid tagID = TagReference[bcr.Ref];
                            PostRepository.AddPostToTag(newPost.ID, tagID);
                            PostRepository.SubmitChanges();
                        }

                        Guid anonUserGuid = MembershipRepository.GetAnonymousUser().ID;

                        foreach (BlogMLComment comment in post.Comments)
                        {
                            IComment cmnt = PostRepository.CreateComment();
                            ICommentAnonymous ica = PostRepository.CreateCommentAnonymous();

                            cmnt.PostID = newPost.ID;
                            cmnt.LanguageID = defaultLanguageID;

                            if (comment.Approved)
                            {
                                cmnt.State = 1;
                            }
                            else
                            {
                                cmnt.State = (byte)EntityState.PendingApproval;
                            }

                            cmnt.Body = comment.Content.Text;
                            cmnt.Created = comment.DateCreated;
                            cmnt.UserAgent = "";
                            cmnt.ID = Guid.NewGuid();

                            if (comment.UserEMail == user.Email ||
                                comment.UserEMail == blog.Authors[0].Email)
                            {
                                cmnt.CreatorUserID = user.ID;
                            }
                            else
                            {
                                cmnt.CreatorUserID = anonUserGuid;
                                ica.CommentID = cmnt.ID;

                                if (!String.IsNullOrEmpty(comment.UserEMail))
                                {
                                    if (comment.UserEMail.Length > 100)
                                    {
                                        ica.Email = comment.UserEMail.Substring(0, 100);
                                    }
                                    else
                                    {
                                        ica.Email = comment.UserEMail;
                                    }

                                    ica.HashedEmail = ica.Email.ComputeHash();
                                }
                                else
                                {
                                    ica.Email = ica.HashedEmail = "";
                                }

                                if (!String.IsNullOrEmpty(comment.UserName))
                                {
                                    if (comment.UserName.Length > 50)
                                    {
                                        ica.Name = comment.UserName.Substring(0, 50);
                                    }
                                    else
                                    {
                                        ica.Name = comment.UserName;
                                    }
                                }
                                else
                                {
                                    ica.Name = "";
                                }

                                if (!String.IsNullOrEmpty(comment.UserUrl))
                                {
                                    if (comment.UserUrl.Length > 300)
                                    {
                                        ica.Url = comment.UserUrl.Substring(0, 300);
                                    }
                                    else
                                    {
                                        ica.Url = comment.UserUrl;
                                    }
                                }
                                else
                                {
                                    ica.Url = "";
                                }
                            }

                            PostRepository.AddComment(cmnt, ica, null, null);
                            PostRepository.SubmitChanges();
                        }
                    }

                    ViewData["Message"] = Localize("BlogML successfully imported");
                }
                catch (Exception ex)
                {
                    ViewData["Message"] = Localize("Error saving:") + "\r\n\r\n" + ex.ToString();
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
            else
            {
                ViewData["Message"] =
                    string.Format(Localize("Warning: Importing BlogML will wipe out all data for the '{0}' area!"),
                                  area.Name);
            }

            return View();
        }
    }
}