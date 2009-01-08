//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Web.Routing;
using Oxite.Configuration;
using Oxite.Data;
using Oxite.Handlers;

namespace Oxite.Routing
{
    public class OxiteLegacyRoutes : OxiteRoutes
    {
        public OxiteLegacyRoutes(RouteCollection routes, IOxiteConfiguration config, IAreaRepository areaRepository,
                                 Type routeHandlerType)
            : base(routes, config, areaRepository, routeHandlerType)
        {
        }

        protected override void RegisterPageRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "Page",
                "oxite.aspx/{*pagePath}",
                new {controller = "Page", action = "Index", mode = RequestMode.View},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageAdd",
                "oxite.aspx/add",
                new {controller = "Page", action = "Index", pagePath = string.Empty, mode = RequestMode.Add},
                null,
                new { Namespaces = controllerNamespaces }
                );
        }

        protected override void RegisterMetaweblogRoutes()
        {
            Routes.Add(
                "MetaWeblogApi",
                new Route(
                    "oxite.aspx/MetaWeblogApi",
                    new RouteValueDictionary(),
                    new RouteValueDictionary(new {api = new HttpMethodConstraint("POST")}),
                    new XmlRpcRouteHandler<MetaWeblogService>()
                    )
                );
        }

        protected override void RegisterArchiveRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "FullArchive",
                "oxite.aspx/Archive/{*archiveData}",
                new {controller = "Archive", action = "Index", archiveData = ArchiveData.DefaultString},
                new {archiveData = new IsArchiveData()},
                new { Namespaces = controllerNamespaces }
                );
        }

        protected override void RegisterHomeRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "HomeDefault",
                "",
                new {controller = "Home", action = "Index", page = 1},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "Home",
                "oxite.aspx",
                new {controller = "Home", action = "Index", page = 1},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageOfAHome",
                "oxite.aspx/page{page}",
                new {controller = "Home", action = "Index", page = 1},
                new {page = new IsInt()},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AllCommentsRss",
                "oxite.aspx/Comments/RSS",
                new {controller = "Home", action = "AllCommentsRss"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AllCommentsAtom",
                "oxite.aspx/Comments/ATOM",
                new {controller = "Home", action = "AllCommentsAtom"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "HomeAtom",
                "oxite.aspx/ATOM",
                new {controller = "Home", action = "IndexAtom"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "HomeRss",
                "oxite.aspx/RSS",
                new {controller = "Home", action = "IndexRss"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "HomeRsd",
                "oxite.aspx/RSD.xml",
                new {controller = "Home", action = "Rsd"},
                null,
                new { Namespaces = controllerNamespaces }
                );
        }

        protected override void RegisterAreaRoutes(string areasConstraint, string[] controllerNamespaces)
        {
            AddRoute(
                "Blog",
                "oxite.aspx/{areaName}",
                new {controller = "Area", action = "Index", page = 1},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageOfABlog",
                "oxite.aspx/{areaName}/page{page}",
                new {controller = "Area", action = "Index", page = 1},
                new {areaName = areasConstraint, page = new IsInt()},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogArchive",
                "oxite.aspx/{areaName}/Archive/{*archiveData}",
                new {controller = "Archive", action = "Index", archiveData = ArchiveData.DefaultString},
                new {areaName = areasConstraint, archiveData = new IsArchiveData()},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogAtom",
                "oxite.aspx/{areaName}/ATOM",
                new {controller = "Area", action = "IndexAtom"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogRss",
                "oxite.aspx/{areaName}/RSS",
                new {controller = "Area", action = "IndexRss"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogAllCommentsRss",
                "oxite.aspx/{areaName}/Comments/RSS",
                new {controller = "Area", action = "AllCommentsRss"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }

                );

            AddRoute(
                "BlogAllCommentsAtom",
                "oxite.aspx/{areaName}/Comments/ATOM",
                new {controller = "Area", action = "AllCommentsAtom"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogRsd",
                "oxite.aspx/{areaName}/RSD.xml",
                new {controller = "Area", action = "Rsd"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogCommentPermalink",
                "oxite.aspx/{areaName}/{slug}#{comment}",
                new {controller = "Area", action = "Permalink"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogCommentPendingPermalink",
                "oxite.aspx/{areaName}/{slug}/pending",
                new {controller = "Area", action = "Permalink", commentState = EntityState.PendingApproval},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogPermalink",
                "oxite.aspx/{areaName}/{slug}",
                new {controller = "Area", action = "Permalink"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogPermalinkAtom",
                "oxite.aspx/{areaName}/{slug}/ATOM",
                new {controller = "Area", action = "PermalinkAtom"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "BlogPermalinkRss",
                "oxite.aspx/{areaName}/{slug}/RSS",
                new {controller = "Area", action = "PermalinkRss"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "ComputeHash",
                "oxite.aspx/ComputeHash",
                new { controller = "Area", action = "ComputeHash" },
                null,
                new { Namespaces = controllerNamespaces }
                );
        }

        protected override void RegisterSearchRoutes(string[] controllerNamespaces)
        {
            if (Config.Site.IncludeOpenSearch)
            {
                AddRoute(
                    "OpenSearch",
                    "oxite.aspx/OpenSearch.xml",
                    new {controller = "Search", action = "OpenSearch"},
                    null,
                    new { Namespaces = controllerNamespaces }
                    );
            }

            AddRoute(
                "Search",
                "oxite.aspx/Search",
                new {controller = "Search", action = "Index", page = 1, count = 10},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageOfASearch",
                "oxite.aspx/Search/page{page}",
                new {controller = "Search", action = "Index", page = 1, count = 10},
                new {page = new IsInt()},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "SearchAtom",
                "oxite.aspx/Search/ATOM",
                new {controller = "Search", action = "IndexAtom"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "SearchRss",
                "oxite.aspx/Search/RSS",
                new {controller = "Search", action = "IndexRss"},
                null,
                new { Namespaces = controllerNamespaces }
                );
        }

        protected override void RegisterUserRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "UserProfile",
                "oxite.aspx/Users/{username}",
                new {controller = "Account", action = "Profile"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "UserProfileEdit",
                "oxite.aspx/Users/{username}/Edit",
                new {controller = "Account", action = "ProfileEdit"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "UserFile",
                "oxite.aspx/Users/{username}/Files/{*filePath}",
                new {controller = "File", action = "UserFile"},
                null,
                new { Namespaces = controllerNamespaces }
                );
        }

        protected override void RegisterLinkbackRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "Pingback",
                "oxite.aspx/{id}/Pingback",
                new {controller = "Pingback", action = "Index"},
                new {id = new IsGuid()},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "Trackback",
                "oxite.aspx/{id}/Trackback",
                new {controller = "Trackback", action = "Index"},
                new {id = new IsGuid()},
                new { Namespaces = controllerNamespaces }
                );
        }

        protected override void RegisterAccountRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "SignIn",
                "oxite.aspx/SignIn",
                new {controller = "Account", action = "SignIn"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "SignOut",
                "oxite.aspx/SignOut",
                new {controller = "Account", action = "SignOut"},
                null,
                new { Namespaces = controllerNamespaces }
                );
        }

        protected override void RegisterTagRoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "Tags",
                "oxite.aspx/Tags",
                new {controller = "Tag", action = "Index"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "TagPermalinkAllCommentsRss",
                "oxite.aspx/Tags/{tagName}/Comments/RSS",
                new {controller = "Tag", action = "AllCommentsRss"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "TagPermalinkAllCommentsAtom",
                "oxite.aspx/Tags/{tagName}/Comments/RSS",
                new {controller = "Tag", action = "AllCommentsAtom"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "TagPermalink",
                "oxite.aspx/Tags/{tagName}",
                new {controller = "Tag", action = "Permalink", page = 1},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageOfATagPermalink",
                "oxite.aspx/Tags/{tagName}/page{page}",
                new {controller = "Tag", action = "Permalink", page = 1},
                new {page = new IsInt()},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "TagPermalinkAtom",
                "oxite.aspx/Tags/{tagName}/ATOM",
                new {controller = "Tag", action = "PermalinkAtom"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "TagPermalinkRss",
                "oxite.aspx/Tags/{tagName}/RSS",
                new {controller = "Tag", action = "PermalinkRss"},
                null,
                new { Namespaces = controllerNamespaces }
                );
        }

        protected override void RegisterSEORoutes(string[] controllerNamespaces)
        {
            AddRoute(
                "Robots.txt",
                "oxite.aspx/robots.txt",
                new {controller = "SEO", action = "Robots"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "SiteMapIndex",
                "oxite.aspx/SiteMap",
                new {controller = "SEO", action = "Index"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "SiteMap",
                "oxite.aspx/SiteMap/{year}/{month}",
                new {controller = "SEO", action = "Permalink"},
                new
                {
                    year = new IsInt(DateTime.MinValue.Year, DateTime.MaxValue.Year),
                    month = new IsInt(DateTime.MinValue.Month, DateTime.MaxValue.Month)
                },
                new { Namespaces = controllerNamespaces }
                );
        }

        protected override void RegisterAdminRoutes(string areasConstraint, string[] controllerNamespaces)
        {
            AddRoute(
                "AdminHome",
                "oxite.aspx/Admin",
                new {controller = "Admin", action = "Index", page = 1},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminSettings",
                "oxite.aspx/Admin/Settings",
                new {controller = "Admin", action = "Settings"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminBlogML",
                "oxite.aspx/Admin/{areaName}/BlogML",
                new {controller = "Admin", action = "BlogML"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageOfAnAdminHome",
                "oxite.aspx/Admin/page{page}",
                new {controller = "Admin", action = "Index", page = 1},
                new {page = new IsInt()},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminFilesAction",
                "oxite.aspx/Admin/Files/{type}",
                new {controller = "Admin", action = "FileModify"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminFiles",
                "oxite.aspx/Admin/Files",
                new {controller = "Admin", action = "Files"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminAreasAdd",
                "oxite.aspx/Admin/Areas/Add",
                new {controller = "Admin", action = "AreasEdit", areaID = Guid.Empty},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminAreasEdit",
                "oxite.aspx/Admin/Areas/{areaID}/Edit",
                new {controller = "Admin", action = "AreasEdit"},
                new {areaID = new IsGuid()},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminAreas",
                "oxite.aspx/Admin/Areas",
                new {controller = "Admin", action = "Areas"},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminBlogPermalinkAdd",
                "oxite.aspx/Admin/{areaName}/Add",
                new {controller = "Admin", action = "ViewPost", mode = RequestMode.Add},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminBlogPermalinkEdit",
                "oxite.aspx/Admin/{areaName}/{slugToEdit}/Edit",
                new {controller = "Admin", action = "ViewPost", mode = RequestMode.Edit},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminBlogPermalinkRemove",
                "oxite.aspx/Admin/{areaName}/{slug}/Remove",
                new {controller = "Admin", action = "RemovePost"},
                new {areaName = areasConstraint},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminComments",
                "oxite.aspx/Admin/Comments",
                new {controller = "Admin", action = "Comments", page = 1},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "PageOfAnAdminComments",
                "oxite.aspx/Admin/Comments/page{page}",
                new {controller = "Admin", action = "Comments", page = 1},
                new {page = new IsInt()},
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminRemoveComment",
                "oxite.aspx/Admin/Comment/Remove",
                new {controller = "Admin", action = "SetCommentState", commentState = EntityState.Removed},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminApproveComment",
                "oxite.aspx/Admin/Comment/Approve",
                new {controller = "Admin", action = "SetCommentState", commentState = EntityState.Normal},
                null,
                new { Namespaces = controllerNamespaces }
                );

            AddRoute(
                "AdminAddPost",
                "oxite.aspx/Admin/AddPost",
                new {controller = "Admin", action = "ViewPost", mode = RequestMode.Add},
                null,
                new { Namespaces = controllerNamespaces }
                );
        }
    }
}