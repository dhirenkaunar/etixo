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
using System.Web.Script.Serialization;
using System.Xml.Linq;
using Oxite.Configuration;
using Oxite.Data;

namespace Oxite.Mvc.Controllers
{
    public class BaseController : Controller
    {
        protected List<HeadLink> feedDiscovery;
        private IUser user;

        public BaseController()
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

        public BaseController(RouteCollection routes, IOxiteConfiguration config, NameValueCollection appSettings,
                              IAreaRepository areaRepository, IMembershipRepository membershipRepository,
                              IPostRepository postRepository, IResourceRepository resourceRepository)
        {
            Routes = routes;
            Config = config;
            AppSettings = new AppSettingsHelper(appSettings);
            PageTitle = new PageTitleHelper(Config);
            AreaRepository = areaRepository;
            MembershipRepository = membershipRepository;
            PostRepository = postRepository;
            ResourceRepository = resourceRepository;
        }

        public bool IsDebugging { get; private set; }
        public RouteCollection Routes { get; protected set; }
        public IOxiteConfiguration Config { get; protected set; }
        public AppSettingsHelper AppSettings { get; protected set; }
        public PageTitleHelper PageTitle { get; protected set; }
        protected IAreaRepository AreaRepository { get; set; }
        protected IMembershipRepository MembershipRepository { get; set; }
        protected IPostRepository PostRepository { get; set; }
        protected IResourceRepository ResourceRepository { get; set; }

        public new IUser User
        {
            get
            {
                if (user == null)
                {
                    if (base.User.Identity.IsAuthenticated)
                    {
                        user = MembershipRepository.GetUser(base.User.Identity.Name);
                    }
                    else
                    {
                        user = MembershipRepository.GetAnonymousUser();
                    }
                }

                return user;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RegisterFeed("All", f => Url.RouteUrl(string.Format(f, "Home")), null);

            base.OnActionExecuting(filterContext);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewData["Title"] = PageTitle.ToString();
            ViewData["UserCanSeeAdmin"] = !User.IsAnonymous &&
                                          AreaRepository.GetAllowedAreas(Config.Site.ID, User.ID).Any();
            ViewData["FeedDiscovery"] = feedDiscovery;
            if (Config.Site.IncludeOpenSearch)
            {
                ViewData["OpenSearch.Title"] = Config.Site.Name + " Search";
                ViewData["OpenSearch.Url"] = Url.RouteUrl("OpenSearch");
            }
#if DEBUG
            IsDebugging = true;
#else
            IsDebugging = false;
#endif

            base.OnActionExecuted(filterContext);
        }

        protected virtual NotFoundResult NotFound()
        {
            return NotFound("");
        }

        protected virtual NotFoundResult NotFound(string description)
        {
            return NotFound(description, "Page not found");
        }

        protected virtual NotFoundResult NotFound(string description, string title)
        {
            if (PageTitle.AdditionalPageTitleSegments != null)
            {
                List<string> segments = new List<string>(PageTitle.AdditionalPageTitleSegments);

                segments.Add(title);

                PageTitle.AdditionalPageTitleSegments = segments.ToArray();
            }
            else
            {
                PageTitle.AdditionalPageTitleSegments = new string[] {title};
            }
            ViewData["Description"] = description;

            return new NotFoundResult();
        }

        protected virtual FileResult File(IFileResource fileResource)
        {
            return new FileResult(fileResource);
        }

        protected virtual XmlViewResult Xml()
        {
            return Xml("");
        }

        protected virtual XmlViewResult Xml(string viewName)
        {
            return new XmlViewResult(viewName);
        }

        protected virtual XmlActionResult Xml(XDocument document)
        {
            return new XmlActionResult(document);
        }

        protected virtual FeedResult Feed(IEnumerable<IFeedItem> feedItems)
        {
            return Feed(feedItems, "");
        }

        protected virtual FeedResult Feed(IEnumerable<IFeedItem> feedItems, string viewName)
        {
            return new FeedResult(feedItems, viewName);
        }

        //TODO: (erikpo) Need to move into BaseViewPage
        protected virtual void RegisterFeed(string title, Func<string, string> generateUrl)
        {
            RegisterFeed(title, generateUrl, null);
        }

        protected virtual void RegisterFeed(string title, Func<string, string> generateUrl, int? insertAtIndex)
        {
            registerFeed(title + " (atom)", "application/atom+xml", generateUrl("{0}Atom"),
                         insertAtIndex.HasValue ? insertAtIndex.Value + 1 : (int?)null);
            registerFeed(title + " (rss)", "application/rss+xml", generateUrl("{0}Rss"),
                         insertAtIndex.HasValue ? insertAtIndex.Value + 1 : (int?)null);
            //TODO: (erikpo) add feed link to some control that shows feeds on the page
        }

        private void registerFeed(string title, string type, string url, int? insertAtIndex)
        {
            if (feedDiscovery == null)
            {
                feedDiscovery = new List<HeadLink>(5);
            }

            HeadLink headLink = new HeadLink() {Rel = "alternate", Href = url, Type = type, Title = title};

            if (!insertAtIndex.HasValue || insertAtIndex.Value >= feedDiscovery.Count)
            {
                feedDiscovery.Add(headLink);
            }
            else
            {
                feedDiscovery.Insert(insertAtIndex.Value, headLink);
            }
        }

        //TODO: (erikpo) Need to move into BaseViewPage
        protected virtual string GenerateRsdLink()
        {
            return GenerateRsdLink(null);
        }

        protected virtual string GenerateRsdLink(IArea area)
        {
            UriBuilder uriBuilder = new UriBuilder(Request.Url.Scheme, Request.Url.Host, Request.Url.Port);

            if (area != null)
            {
                uriBuilder.Path = Url.RouteUrl(string.Format("{0}Rsd", area.Type), new {areaName = area.Name});
            }
            else
            {
                uriBuilder.Path = Url.RouteUrl("HomeRsd");
            }

            return uriBuilder.Uri.ToString();
        }

        protected virtual XDocument GenerateRsd()
        {
            return GenerateRsd(null);
        }

        protected virtual XDocument GenerateRsd(string blogName)
        {
            Uri uri = ControllerContext.HttpContext.Request.Url;
            UriBuilder homepageLink = new UriBuilder(uri.Scheme, uri.Host, uri.Port);
            UriBuilder apiLink = new UriBuilder(homepageLink.Uri);

            apiLink.Path = Url.RouteUrl("MetaWeblogApi");

            XNamespace rsdNamespace = "http://archipelago.phrasewise.com/rsd";
            XDocument rsd = new XDocument(
                new XElement(rsdNamespace + "rsd", new XAttribute("version", "1.0"),
                             new XElement(rsdNamespace + "service",
                                          new XElement(rsdNamespace + "engineName", "Oxite"),
                                          new XElement(rsdNamespace + "engineLink", "http://oxite.net"),
                                          new XElement(rsdNamespace + "homePageLink", homepageLink.Uri.ToString()),
                                          new XElement(rsdNamespace + "apis",
                                                       generateRsdApiList(blogName, rsdNamespace, apiLink))
                                 )
                    )
                );

            return rsd;
        }

        private XElement[] generateRsdApiList(string areaName, XNamespace rsdNamespace, UriBuilder apiLink)
        {
            IEnumerable<IArea> areas = AreaRepository.GetAreas(Config.Site.ID);
            List<XElement> elements = new List<XElement>(areas.Count());

            foreach (IArea area in areas)
            {
                elements.Add(new XElement(rsdNamespace + "api", new XAttribute("name", "MetaWeblog"),
                                          new XAttribute("blogID", area.ID.ToString("N")),
                                          new XAttribute("preferred",
                                                         areas.Count() == 1 ||
                                                         string.Compare(area.Name, areaName, true) == 0
                                                             ? "true"
                                                             : "false"),
                                          new XAttribute("apiLink", apiLink.Uri.ToString())));
            }

            return elements.ToArray();
        }

        public virtual string Localize(string value)
        {
            return Localize(value, false);
        }

        public virtual string Localize(string value, string ns)
        {
            return Localize(value, ns, false);
        }

        public virtual string Localize(string value, bool jsonSerialize)
        {
            return Localize(value, "", jsonSerialize);
        }

        public virtual string Localize(string value, string ns, bool jsonSerialize)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            Dictionary<string, string> stringResources = (Dictionary<string, string>)ViewData["StringResources"];
            List<string> stringResourcesToSerialize = (List<string>)ViewData["StringResourcesToSerialize"];

            if (stringResources == null)
            {
                ViewData["StringResources"] = stringResources = new Dictionary<string, string>(10);
            }
            if (stringResourcesToSerialize == null && jsonSerialize)
            {
                ViewData["StringResourcesToSerialize"] = stringResourcesToSerialize = new List<string>(5);
            }

            string name = !string.IsNullOrEmpty(ns) ? string.Format("{0}.{1}", ns, value) : value;
            string foundValue = stringResources != null && stringResources.ContainsKey(name)
                                    ? stringResources[name]
                                    : null;

            if (foundValue != null)
            {
                return foundValue;
            }
            else
            {
                IStringResource sr = string.Compare(Config.Site.LanguageDefault, User.LanguageDefault.Name, true) != 0
                                         ? ResourceRepository.GetString(name, User.LanguageDefault)
                                         : null;

                if (jsonSerialize)
                {
                    stringResourcesToSerialize.Add(name);
                }

                //TODO: (erikpo) Add some better logic here to get a language that's "close" to the requested language if available

                if (sr != null)
                {
                    stringResources.Add(name, sr.Value);

                    return sr.Value;
                }
                else
                {
                    stringResources.Add(name, value);

                    return value;
                }
            }
        }

        public virtual void RenderStringResources()
        {
            Dictionary<string, string> stringResources = (Dictionary<string, string>)ViewData["StringResources"];
            List<string> stringResourcesToSerialize = (List<string>)ViewData["StringResourcesToSerialize"];

            if (stringResourcesToSerialize != null && stringResourcesToSerialize.Count > 0)
            {
                Dictionary<string, string> srList = new Dictionary<string, string>(stringResourcesToSerialize.Count);

                foreach (string sr in stringResourcesToSerialize)
                {
                    srList.Add(sr, stringResources[sr]);
                }

                if (srList.Count > 0)
                {
                    Response.Write(
                        string.Format("<script type=\"text/javascript\">window.stringResources = {0};</script>",
                                      new JavaScriptSerializer().Serialize(srList)));
                }
            }
        }

        public virtual DateTime ConvertToLocalTime(DateTime dateTime)
        {
            if (ControllerContext.HttpContext.User == null)
            {
                if (Config.Site.TimeZoneOffset != 0)
                {
                    return dateTime.Add(TimeSpan.FromHours(Config.Site.TimeZoneOffset));
                }
                else
                {
                    return dateTime;
                }
            }
            else
            {
                return dateTime; //TODO: (erikpo) Get the timezone offset from the current user and apply it
            }
        }

        public virtual DateTime ConvertFromLocalTime(DateTime dateTime)
        {
            if (ControllerContext.HttpContext.User == null)
            {
                if (Config.Site.TimeZoneOffset != 0)
                {
                    return dateTime.Subtract(TimeSpan.FromHours(Config.Site.TimeZoneOffset));
                }
                else
                {
                    return dateTime;
                }
            }
            else
            {
                return dateTime; //TODO: (erikpo) Get the timezone offset from the current user and apply it
            }
        }

        public virtual string GetAbsolutePath(string relativeUrl)
        {
            UriBuilder uriBuilder = new UriBuilder(Request.Url.Scheme, Request.Url.Host, Request.Url.Port);

            uriBuilder.Path = relativeUrl;

            return uriBuilder.Uri.ToString();
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            if (model != null)
            {
                ViewData.Model = model;
            }

            return new OxiteViewResult()
                   {
                       ViewName = viewName,
                       MasterName = masterName,
                       ViewData = ViewData,
                       TempData = TempData
                   };
        }

        protected override ViewResult View(IView view, object model)
        {
            if (model != null)
            {
                ViewData.Model = model;
            }

            return new OxiteViewResult()
                   {
                       View = view,
                       ViewData = ViewData,
                       TempData = TempData
                   };
        }
    }
}