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
using Oxite.Search;

namespace Oxite.Mvc.Controllers
{
    public class SearchController : BaseController
    {
        public SearchController()
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

            SearchProvider = Config.SearchProvider.GetInstance();
            SearchProviderProperties = Config.SearchProvider.ConfigurationProperties;
        }

        public SearchController(RouteCollection routes, IOxiteConfiguration config, NameValueCollection appSettings,
                                IAreaRepository areaRepository, IMembershipRepository membershipRepository,
                                IPostRepository postRepository, IResourceRepository resourceRepository,
                                ISearchProvider searchProvider, NameValueCollection searchProviderProperties)
            : base(routes, config, appSettings, areaRepository, membershipRepository, postRepository, resourceRepository
                )
        {
            SearchProvider = searchProvider;
            SearchProviderProperties = searchProviderProperties;
        }

        protected static ISearchProvider SearchProvider { get; set; }
        protected static NameValueCollection SearchProviderProperties { get; set; }

        public virtual ActionResult Index(string term, int page, int count)
        {
            PageOfAList<ISearchResultItem> results = !string.IsNullOrEmpty(term)
                                                         ? SearchProvider.GetSearchResults(term, page - 1, count,
                                                                                           SearchProviderProperties)
                                                         : null;
            Dictionary<Guid, int> resultCounts = null;

            PageTitle.AdditionalPageTitleSegments = new string[] {"Search"};

            if (results != null && results.Count > 0)
            {
                resultCounts = new Dictionary<Guid, int>(results.Count);
                results.ToList().ForEach(p => resultCounts.Add(p.ID, PostRepository.GetComments(p).Count()));
            }

            ViewData["AreaCount"] = AreaRepository.GetAreasCount(Config.Site.ID);
            ViewData["SearchResults"] = results;
            ViewData["PostCounts"] = resultCounts;
            ViewData["Title"] = PageTitle.ToString();

            return View();
        }

        public virtual FeedResult IndexRss(string term)
        {
            return IndexFeed(term);
        }

        public virtual FeedResult IndexAtom(string term)
        {
            return IndexFeed(term);
        }

        public virtual FeedResult IndexFeed(string term)
        {
            PageTitle.AdditionalPageTitleSegments = new string[] {string.Format(Localize("Searched for '{0}'"), term)};

            ViewData["Term"] = term;

            return Feed(SearchProvider.GetSearchResults(term, 0, 50, SearchProviderProperties).Cast<IFeedItem>());
        }

        public virtual ActionResult OpenSearch()
        {
            ViewData["OpenSearch.ShortName"] = string.Format(Localize("{0} Search"), Config.Site.Name);
            ViewData["OpenSearch.Description"] = string.Format(Localize("Search the content on {0}"), Config.Site.Name);

            return Xml();
        }
    }
}