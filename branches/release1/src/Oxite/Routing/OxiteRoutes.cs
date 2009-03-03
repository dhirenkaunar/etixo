//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Linq;
using System.Web.Routing;
using Oxite.Configuration;
using Oxite.Data;
using Oxite.Handlers;

namespace Oxite.Routing
{
    public class OxiteRoutes : IRoutingRegistration
    {
        private IAreaRepository areaRepository;
        private IOxiteConfiguration config;
        private Type routeHandlerType;
        private RouteCollection routes;

        public OxiteRoutes(RouteCollection routes, IOxiteConfiguration config, IAreaRepository areaRepository,
                           Type routeHandlerType)
        {
            this.routes = routes;
            this.config = config;
            this.areaRepository = areaRepository;
            this.routeHandlerType = routeHandlerType;
        }

        protected RouteCollection Routes
        {
            get
            {
                return routes;
            }
            set
            {
                routes = value;
            }
        }

        protected IOxiteConfiguration Config
        {
            get
            {
                return config;
            }
            set
            {
                config = value;
            }
        }

        protected IAreaRepository AreaRepository
        {
            get
            {
                return areaRepository;
            }
            set
            {
                areaRepository = value;
            }
        }

        protected Type RouteHandlerType
        {
            get
            {
                return routeHandlerType;
            }
            set
            {
                routeHandlerType = value;
            }
        }

        public virtual void RegisterRoutes()
        {
            string[] areas = AreaRepository.GetAreas(Config.Site.ID).Select(a => a.Name).ToArray();
            string areasConstraint = areas != null && areas.Length > 0
                                         ? areas.Length > 1 ? string.Format("({0})", string.Join("|", areas)) : areas[0]
                                         : "";

            Routes.Clear();

            Routes.Add(new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler()));

            RegisterRoutes(Routes, areasConstraint, Config.ControllerNamespaces);
        }

        protected virtual void RegisterRoutes(RouteCollection routes, string areasConstraint,
                                              string[] controllerNamespaces)
        {
            RegisterHomeRoutes(controllerNamespaces);

            RegisterAdminRoutes(areasConstraint, controllerNamespaces);

            RegisterSearchRoutes(controllerNamespaces);

            RegisterAreaRoutes(areasConstraint, controllerNamespaces);

            RegisterUserRoutes(controllerNamespaces);

            RegisterTagRoutes(controllerNamespaces);

            RegisterArchiveRoutes(controllerNamespaces);

            RegisterLinkbackRoutes(controllerNamespaces);

            RegisterSEORoutes(controllerNamespaces);

            RegisterAccountRoutes(controllerNamespaces);

            RegisterMetaweblogRoutes();

            //INFO: (erikpo) This route must remain last
            RegisterPageRoutes(controllerNamespaces);
        }

        protected virtual void RegisterPageRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "Page",
                "{*pagePath}",
                new {controller = "Page", action = "Index", mode = RequestMode.View},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageAdd",
                "add",
                new {controller = "Page", action = "Index", pagePath = string.Empty, mode = RequestMode.Add},
                null,
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterMetaweblogRoutes()
        {
            Routes.Add(
                "MetaWeblogApi",
                new Route(
                    "MetaWeblogApi",
                    new RouteValueDictionary(),
                    new RouteValueDictionary(new {api = new HttpMethodConstraint("POST")}),
                    new XmlRpcRouteHandler<MetaWeblogService>()
                    )
                );
        }

        protected virtual void RegisterArchiveRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "FullArchive",
                "Archive/{*archiveData}",
                new {controller = "Archive", action = "Index", archiveData = ArchiveData.DefaultString},
                new {archiveData = new IsArchiveData()},
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterHomeRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "Home",
                "",
                new {controller = "Home", action = "Index", page = 1},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageOfAHome",
                "page{page}",
                new {controller = "Home", action = "Index", page = 1},
                new {page = new IsInt()},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AllCommentsRss",
                "Comments/RSS",
                new {controller = "Home", action = "AllCommentsRss"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AllCommentsAtom",
                "Comments/ATOM",
                new {controller = "Home", action = "AllCommentsAtom"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "HomeAtom",
                "ATOM",
                new {controller = "Home", action = "IndexAtom"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "HomeRss",
                "RSS",
                new {controller = "Home", action = "IndexRss"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "HomeRsd",
                "RSD.xml",
                new {controller = "Home", action = "Rsd"},
                null,
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterAreaRoutes(string areasConstraint, string[] controllerNamespaces)
        {
            AddRoute(
                "Blog",
                "{areaName}",
                new {controller = "Area", action = "Index", page = 1},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageOfABlog",
                "{areaName}/page{page}",
                new {controller = "Area", action = "Index", page = 1},
                new {areaName = areasConstraint, page = new IsInt()},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogArchive",
                "{areaName}/Archive/{*archiveData}",
                new {controller = "Archive", action = "Index", archiveData = ArchiveData.DefaultString},
                new {areaName = areasConstraint, archiveData = new IsArchiveData()},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogAtom",
                "{areaName}/ATOM",
                new {controller = "Area", action = "IndexAtom"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogRss",
                "{areaName}/RSS",
                new {controller = "Area", action = "IndexRss"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogAllCommentsRss",
                "{areaName}/Comments/RSS",
                new {controller = "Area", action = "AllCommentsRss"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogAllCommentsAtom",
                "{areaName}/Comments/ATOM",
                new {controller = "Area", action = "AllCommentsAtom"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogRsd",
                "{areaName}/RSD.xml",
                new {controller = "Area", action = "Rsd"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogCommentPermalink",
                "{areaName}/{slug}#{comment}",
                new {controller = "Area", action = "Permalink"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogCommentPendingPermalink",
                "{areaName}/{slug}/pending",
                new {controller = "Area", action = "Permalink", commentState = EntityState.PendingApproval},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogPermalink",
                "{areaName}/{slug}",
                new {controller = "Area", action = "Permalink"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogPermalinkAtom",
                "{areaName}/{slug}/ATOM",
                new {controller = "Area", action = "PermalinkAtom"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogPermalinkRss",
                "{areaName}/{slug}/RSS",
                new {controller = "Area", action = "PermalinkRss"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "ComputeHash",
                "ComputeHash",
                new { controller = "Area", action = "ComputeHash" },
                null,
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterSearchRoutes(string[] controllerNamespaces)
        {
            if (Config.Site.IncludeOpenSearch)
            {
                AddRoute(
                    "OpenSearch",
                    "OpenSearch.xml",
                    new {controller = "Search", action = "OpenSearch"},
                    null,
                    new { Namespaces = controllerNamespaces }
                    );
            }

            AddRoute(
                "Search",
                "Search",
                new {controller = "Search", action = "Index", page = 1, count = 10},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageOfASearch",
                "Search/page{page}",
                new {controller = "Search", action = "Index", page = 1, count = 10},
                new {page = new IsInt()},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "SearchAtom",
                "Search/ATOM",
                new {controller = "Search", action = "IndexAtom"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "SearchRss",
                "Search/RSS",
                new {controller = "Search", action = "IndexRss"},
                null,
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterUserRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "UserProfile",
                "Users/{username}",
                new {controller = "Account", action = "Profile"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "UserProfileEdit",
                "Users/{username}/Edit",
                new {controller = "Account", action = "ProfileEdit"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "UserFile",
                "Users/{username}/Files/{*filePath}",
                new {controller = "File", action = "UserFile"},
                null,
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterLinkbackRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "Pingback",
                "{id}/Pingback",
                new {controller = "Pingback", action = "Index"},
                new {id = new IsGuid()},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "Trackback",
                "{id}/Trackback",
                new {controller = "Trackback", action = "Index"},
                new {id = new IsGuid()},
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterAccountRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "SignIn",
                "SignIn",
                new {controller = "Account", action = "SignIn"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "SignOut",
                "SignOut",
                new {controller = "Account", action = "SignOut"},
                null,
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterTagRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "Tags",
                "Tags",
                new {controller = "Tag", action = "Index"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "TagPermalinkAllCommentsRss",
                "Tags/{tagName}/Comments/RSS",
                new {controller = "Tag", action = "AllCommentsRss"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "TagPermalinkAllCommentsAtom",
                "Tags/{tagName}/Comments/RSS",
                new {controller = "Tag", action = "AllCommentsAtom"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "TagPermalink",
                "Tags/{tagName}",
                new {controller = "Tag", action = "Permalink", page = 1},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageOfATagPermalink",
                "Tags/{tagName}/page{page}",
                new {controller = "Tag", action = "Permalink", page = 1},
                new {page = new IsInt()},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "TagPermalinkAtom",
                "Tags/{tagName}/ATOM",
                new {controller = "Tag", action = "PermalinkAtom"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "TagPermalinkRss",
                "Tags/{tagName}/RSS",
                new {controller = "Tag", action = "PermalinkRss"},
                null,
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterSEORoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "Robots.txt",
                "robots.txt",
                new {controller = "SEO", action = "Robots"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "SiteMapIndex",
                "SiteMap",
                new {controller = "SEO", action = "Index"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "SiteMap",
                "SiteMap/{year}/{month}",
                new {controller = "SEO", action = "Permalink"},
                new
                {
                    year = new IsInt(DateTime.MinValue.Year, DateTime.MaxValue.Year),
                    month = new IsInt(DateTime.MinValue.Month, DateTime.MaxValue.Month)
                },
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void RegisterAdminRoutes(string areasConstraint, string[] controllerNamespaces)
        {
            AddRoute(
                "AdminHome",
                "Admin",
                new {controller = "Admin", action = "Index", page = 1},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminSettings",
                "Admin/Settings",
                new {controller = "Admin", action = "Settings"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminBlogML",
                "Admin/{areaName}/BlogML",
                new {controller = "Admin", action = "BlogML"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageOfAnAdminHome",
                "Admin/page{page}",
                new {controller = "Admin", action = "Index", page = 1},
                new {page = new IsInt()},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminFilesAction",
                "Admin/Files/{type}",
                new {controller = "Admin", action = "FileModify"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminFiles",
                "Admin/Files",
                new {controller = "Admin", action = "Files"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminAreasAdd",
                "Admin/Areas/Add",
                new {controller = "Admin", action = "AreasEdit", areaID = Guid.Empty},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminAreasEdit",
                "Admin/Areas/{areaID}/Edit",
                new {controller = "Admin", action = "AreasEdit"},
                new {areaID = new IsGuid()},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminAreas",
                "Admin/Areas",
                new {controller = "Admin", action = "Areas"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminBlogPermalinkAdd",
                "Admin/{areaName}/Add",
                new {controller = "Admin", action = "ViewPost", mode = RequestMode.Add},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminBlogPermalinkEdit",
                "Admin/{areaName}/{slugToEdit}/Edit",
                new {controller = "Admin", action = "ViewPost", mode = RequestMode.Edit},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminBlogPermalinkRemove",
                "Admin/{areaName}/{slug}/Remove",
                new {controller = "Admin", action = "RemovePost"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminComments",
                "Admin/Comments",
                new {controller = "Admin", action = "Comments", page = 1},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageOfAnAdminComments",
                "Admin/Comments/page{page}",
                new {controller = "Admin", action = "Comments", page = 1},
                new {page = new IsInt()},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminRemoveComment",
                "Admin/Comment/Remove",
                new {controller = "Admin", action = "SetCommentState", commentState = EntityState.Removed},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminApproveComment",
                "Admin/Comment/Approve",
                new {controller = "Admin", action = "SetCommentState", commentState = EntityState.Normal},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminAddPost",
                "Admin/AddPost",
                new { controller = "Admin", action = "ViewPost", mode = RequestMode.Add },
                null,
                new { Namespaces = controllerNamespaces }
                );
        }

        protected virtual void AddRoute(string name, string url, object defaults, object constraints, object dataTokens)
        {
            Routes.Add(name,
                new Route(url, CreateRouteHandler())
                {
                    Defaults = new RouteValueDictionary(defaults),
                    Constraints = new RouteValueDictionary(constraints),
                    DataTokens = new RouteValueDictionary(dataTokens)
                }
            );
        }

        protected virtual IRouteHandler CreateRouteHandler()
        {
            if (RouteHandlerType != null)
            {
                return (IRouteHandler)Activator.CreateInstance(RouteHandlerType);
            }
            else
            {
                return null;
            }
        }
    }
}