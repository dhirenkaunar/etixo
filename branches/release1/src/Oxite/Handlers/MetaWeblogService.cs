//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Routing;
using CookComputing.XmlRpc;
using Oxite.Configuration;
using Oxite.Data;

namespace Oxite.Handlers
{
    public class MetaWeblogService : XmlRpcHttpServerProtocol, IMetaWeblog
    {
        public MetaWeblogService()
        {
            GetRoutes = () => RouteTable.Routes;
            HttpContext = new HttpContextWrapper(System.Web.HttpContext.Current);
            Config = (IOxiteConfiguration)ConfigurationManager.GetSection("oxite");

            IOxiteDataProvider dataProvider = Config.DataProvider.GetInstance();
            AreaRepository = dataProvider.AreaRepository;
            ResourceRepository = dataProvider.ResourceRepository;
            MembershipRepository = dataProvider.MembershipRepository;
            PostRepository = dataProvider.PostRepository;
            TagRepository = dataProvider.TagRepository;
        }

        public MetaWeblogService(Func<RouteCollection> getRoutes, HttpContextBase httpContext,
                                 IOxiteConfiguration config, IAreaRepository areaRepository,
                                 IResourceRepository resourceRepository, IMembershipRepository membershipRepository,
                                 IPostRepository postRepository, ITagRepository tagRepository)
        {
            GetRoutes = getRoutes;
            HttpContext = httpContext;
            Config = config;
            AreaRepository = areaRepository;
            ResourceRepository = resourceRepository;
            MembershipRepository = membershipRepository;
            PostRepository = postRepository;
            TagRepository = tagRepository;
        }

        protected Func<RouteCollection> GetRoutes { get; private set; }
        protected HttpContextBase HttpContext { get; private set; }
        protected IOxiteConfiguration Config { get; private set; }
        protected IAreaRepository AreaRepository { get; private set; }
        protected IResourceRepository ResourceRepository { get; private set; }
        protected IMembershipRepository MembershipRepository { get; private set; }
        protected IPostRepository PostRepository { get; private set; }
        protected ITagRepository TagRepository { get; private set; }

        #region IMetaWeblog Members

        public string NewPost(string blogId, string username, string password, Post post, bool publish)
        {
            IArea area = GetArea(blogId);
            IUser user = GetUser(username, password);

            VerifyUserAccessToArea(user, area);

            IPost newPost = PostRepository.CreatePost();

            newPost.State = (byte)EntityState.Normal;
            newPost.ID = Guid.NewGuid();

            UpdatePost(post, newPost, user, publish);

            ISubscription subscription = null;

            if (Config.Site.AuthorAutoSubscribe)
            {
                subscription = PostRepository.CreateSubscription();

                subscription.UserID = user.ID;
            }

            PostRepository.AddPost(newPost, subscription);
            AreaRepository.AddPostToArea(newPost.ID, area.ID);

            PostRepository.SubmitChanges();

            return newPost.ID.ToString();
        }

        public bool EditPost(string postId, string username, string password, Post post, bool publish)
        {
            IUser user = GetUser(username, password);
            IPost oldPost = PostRepository.GetPost(new Guid(postId));

            if (oldPost == null)
            {
                throw new XmlRpcFaultException(0, "Invalid post ID");
            }

            VerifyUserAccessToPost(user, oldPost);

            UpdatePost(post, oldPost, user, publish);

            PostRepository.SubmitChanges();

            return true;
        }

        public Post GetPost(string postId, string username, string password)
        {
            IUser user = GetUser(username, password);
            IPost post = PostRepository.GetPost(new Guid(postId));

            if (post == null)
            {
                throw new XmlRpcFaultException(0, "Invalid post ID");
            }

            VerifyUserAccessToPost(user, post);

            string permalink = GetBlogPermalinkUrl(post);

            IUser postUser = post.CreatorUser;
            Post newPost = new Post()
                           {
                               categories = post.Tags.Select(t => t.Name).ToArray(),
                               dateCreated = post.Created.Value,
                               description = post.Body,
                               link = permalink,
                               permalink = permalink,
                               postid = post.ID.ToString(),
                               mt_excerpt = post.BodyShort,
                               mt_basename = post.Slug,
                               title = post.Title,
                               userid = post.CreatorUser.ID.ToString("N"),
                               wp_author = postUser.Username,
                               wp_author_id = post.CreatorUser.ID.ToString("N"),
                               wp_author_display_name = postUser.GetDisplayName()
                           };

            return newPost;
        }

        public UrlData NewMediaObject(string blogId, string username, string password, FileData file)
        {
            IArea area = GetArea(blogId);
            IUser user = GetUser(username, password);

            VerifyUserAccessToArea(user, area);

            IFileResource fileResource = ResourceRepository.CreateFile();

            fileResource.SiteID = Config.Site.ID;
            fileResource.CreatorUser = user;
            fileResource.Name = file.name;
            fileResource.Content = file.bits;
            fileResource.ContentType = file.type;
            fileResource.Path = "";
            fileResource.State = (byte)EntityState.Normal;

            ResourceRepository.AddFile(fileResource);
            ResourceRepository.AddFileToUser(fileResource.ID, user.ID);
            ResourceRepository.SubmitChanges();

            return new UrlData() {url = GetFileResourceUrl(user, fileResource)};
        }

        public CategoryInfo[] GetCategories(string blogId, string username, string password)
        {
            IArea area = GetArea(blogId);
            IUser user = GetUser(username, password);

            VerifyUserAccessToArea(user, area);

            return
                TagRepository.GetTags().Select(
                    t => new CategoryInfo() {description = t.Name, htmlUrl = GetTagUrl(t), rssUrl = GetTagRssUrl(t)}).
                    ToArray();
        }

        public Post[] GetRecentPosts(string blogId, string username, string password, int numberOfPosts)
        {
            IArea area = GetArea(blogId);
            IUser user = GetUser(username, password);

            VerifyUserAccessToArea(user, area);

            List<Post> posts = new List<Post>();

            foreach (IPost post in PostRepository.GetPosts(area, 0, numberOfPosts))
            {
                posts.Add(GetPost(post.ID.ToString(), username, password));
            }

            return posts.ToArray();
        }

        public BlogInfo[] GetUsersBlogs(string apikey, string username, string password)
        {
            IUser user = GetUser(username, password);
            IEnumerable<IArea> areas = AreaRepository.GetAreas(Config.Site.ID);

            IEnumerable<IArea> allowedAreas = areas.Where(a => CanUserAccessArea(user, a));

            if (allowedAreas.Count() == 0)
            {
                throw new XmlRpcFaultException(0, "Invalid username/password.");
            }

            return
                allowedAreas.Select(
                    a => new BlogInfo() {blogid = a.ID.ToString(), blogName = a.Name, url = GetBlogUrl(a)}).ToArray();
        }

        public string NewCategory(string blog_id, string username, string password, NewCategoryInfo newCategory)
        {
            IArea area = GetArea(blog_id);
            IUser user = GetUser(username, password);
            Guid tagID;

            VerifyUserAccessToArea(user, area);

            using (TransactionScope transaction = new TransactionScope())
            {
                ITag oldTag = TagRepository.GetTag(newCategory.name);

                if (oldTag != null)
                {
                    tagID = oldTag.ID;
                }
                else
                {
                    ITag newTag = TagRepository.CreateTag(newCategory.name);

                    TagRepository.AddTag(newTag);

                    TagRepository.SubmitChanges();

                    tagID = newTag.ID;
                }

                transaction.Complete();
            }

            return tagID.ToString();
        }

        public bool DeletePost(string appkey, string postid, string username, string password, bool publish)
        {
            IUser user = GetUser(username, password);
            IPost oldPost = PostRepository.GetPost(new Guid(postid));

            VerifyUserAccessToPost(user, oldPost);

            PostRepository.RemovePost(oldPost.ID);

            PostRepository.SubmitChanges();

            return true;
        }

        public AuthorInfo[] GetAuthors(string blog_id, string username, string password)
        {
            IArea area = GetArea(blog_id);
            IUser user = GetUser(username, password);

            VerifyUserAccessToArea(user, area);

            IEnumerable<IUser> users = MembershipRepository.GetAreaUsers(area);

            return (from u in users
                    select new AuthorInfo()
                           {
                               user_id = u.ID.ToString(),
                               user_login = u.Username,
                               display_name = u.GetDisplayName(),
                               user_email = u.HashedEmail
                           }).ToArray();
        }

        #endregion

        private IArea GetArea(string areaID)
        {
            Guid areaIDValue;

            if (!areaID.GuidTryParse(out areaIDValue))
            {
                throw new XmlRpcFaultException(0, "Bad blog ID");
            }

            IArea area = AreaRepository.GetArea(areaIDValue);

            if (area == null)
            {
                throw new XmlRpcFaultException(0, "Invalid Blog ID");
            }

            return area;
        }

        private string GetBlogUrl(IArea area)
        {
            return GetAbsolutePath(area.GetUrl(new RequestContext(HttpContext, new RouteData()), GetRoutes()));
        }

        private string GetBlogPermalinkUrl(IPost post)
        {
            return GetAbsolutePath(post.GetAreaPostUrl(new RequestContext(HttpContext, new RouteData()), GetRoutes()));
        }

        private string GetTagUrl(ITag tag)
        {
            return GetAbsolutePath("TagPermalink", new {tagName = tag.Name});
        }

        private string GetTagRssUrl(ITag tag)
        {
            return GetAbsolutePath("TagPermalinkRss", new {tagName = tag.Name});
        }

        private string GetFileResourceUrl(IUser user, IFileResource fileResource)
        {
            string filePath = fileResource.Path;

            if (filePath == "/")
            {
                filePath = "";
            }
            else if (filePath.Length > 1 && !filePath.StartsWith("/"))
            {
                filePath = "/" + filePath;
            }

            if (filePath.EndsWith("/"))
            {
                filePath = filePath.Substring(0, filePath.Length - 1);
            }

            filePath = filePath + "/" + fileResource.Name;

            if (filePath.StartsWith("/"))
            {
                filePath = filePath.Substring(1);
            }

            return GetAbsolutePath("UserFile", new {username = user.Username, filePath = filePath});
        }

        private string GetVirtualPath(string routeName, object values)
        {
            return
                GetRoutes().GetVirtualPath(new RequestContext(HttpContext, new RouteData()), routeName,
                                           new RouteValueDictionary(values)).VirtualPath;
        }

        private string GetAbsolutePath(string routeName, object values)
        {
            return GetAbsolutePath(GetVirtualPath(routeName, values));
        }

        private string GetAbsolutePath(string path)
        {
            Uri uri = HttpContext.Request.Url;
            UriBuilder link = new UriBuilder(uri.Scheme, uri.Host, uri.Port);

            link.Path = path;

            return link.Uri.ToString();
        }

        private void UpdatePost(Post post, IPost oldPost, IUser user, bool publish)
        {
            oldPost.Title = post.title;
            oldPost.Body = post.description;
            oldPost.BodyShort = post.mt_excerpt ?? string.Empty;
            oldPost.Slug = !string.IsNullOrEmpty(post.mt_basename) ? post.mt_basename : oldPost.Title.Slugify();

            if (!oldPost.Created.HasValue)
            {
                if (post.dateCreated != default(DateTime))
                {
                    oldPost.Created = post.dateCreated;
                }
                else
                {
                    oldPost.Created = DateTime.Now.ToUniversalTime();
                }
            }

            if (publish)
            {
                if (post.dateCreated != default(DateTime))
                {
                    oldPost.Published = post.dateCreated;
                }
                else if (!oldPost.Published.HasValue)
                {
                    oldPost.Published = DateTime.Now.ToUniversalTime();
                }
            }

            string[] categories = post.categories ?? new string[0];
            IEnumerable<ITag> newTags =
                categories.Where(s => !string.IsNullOrEmpty(s)).Select(
                    s => TagRepository.GetTag(s) ?? TagRepository.CreateTag(s)).ToList();
            newTags.Where(t => t.ID == Guid.Empty).ToList().ForEach(t => TagRepository.AddTag(t));

            foreach (ITag tag in oldPost.Tags.Except(newTags).ToList())
            {
                PostRepository.RemovePostFromTag(oldPost.ID, tag.ID);
            }

            foreach (ITag tag in newTags.Except(oldPost.Tags).ToList())
            {
                PostRepository.AddPostToTag(oldPost.ID, tag.ID);
            }

            if (string.IsNullOrEmpty(post.wp_author_id))
            {
                oldPost.CreatorUserID = user.ID;
            }
            else
            {
                oldPost.CreatorUserID = new Guid(post.wp_author_id);
            }
        }

        private IUser GetUser(string username, string password)
        {
            IUser user = MembershipRepository.GetUser(username, password);

            if (user == null)
            {
                throw new XmlRpcFaultException(0, "Invalid Username and/or Password");
            }

            return user;
        }

        private void VerifyUserAccessToPost(IUser user, IPost oldPost)
        {
            VerifyUserAccessToArea(user, oldPost.Area);
        }

        private void VerifyUserAccessToAllAreas(IUser user, IEnumerable<IArea> areas)
        {
            foreach (IArea area in areas)
            {
                if (!CanUserAccessArea(user, area))
                {
                    throw new XmlRpcFaultException(0, "Invalid username/password");
                }
            }
        }

        private void VerifyUserAccessToArea(IUser user, IArea area)
        {
            if (!CanUserAccessArea(user, area))
            {
                throw new XmlRpcFaultException(0, "Invalid username/password");
            }
        }

        private bool CanUserAccessArea(IUser user, IArea area)
        {
            return MembershipRepository.GetUserIsInRole(user.ID, area);
        }

        private bool VerifyUserInAtLeastOneRole(IUser user, IEnumerable<string> authorRoles)
        {
            if (authorRoles.Count() > 0 &&
                !authorRoles.Aggregate(false,
                                       (match, roleID) =>
                                       match ? match : MembershipRepository.GetUserIsInRole(user.ID, new Guid(roleID))))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}