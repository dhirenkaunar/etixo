//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Oxite.Configuration;
using Oxite.Data;

namespace Oxite.Mvc.Controllers
{
    public class TrackbackController : Controller
    {
        public TrackbackController()
            : base()
        {
            Config = (IOxiteConfiguration)ConfigurationManager.GetSection("oxite");
            AppSettings = new AppSettingsHelper(ConfigurationManager.AppSettings);

            IOxiteDataProvider dataProvider = Config.DataProvider.GetInstance();

            PostRepository = dataProvider.PostRepository;
            TrackbackRepository = dataProvider.TrackbackRepository;
        }

        public TrackbackController(IOxiteConfiguration config, NameValueCollection appSettings,
                                   IPostRepository postRepository, ITrackbackRepository trackbackRepository)
            : base()
        {
            Config = config;
            AppSettings = new AppSettingsHelper(appSettings);
            PostRepository = postRepository;
            TrackbackRepository = trackbackRepository;
        }

        public IOxiteConfiguration Config { get; private set; }
        public AppSettingsHelper AppSettings { get; private set; }
        protected IPostRepository PostRepository { get; private set; }
        protected ITrackbackRepository TrackbackRepository { get; private set; }

        public virtual ActionResult Index(Guid id)
        {
            if (!Config.Site.TrackbacksEnabled)
            {
                return new TrackbackErrorResult(2, "Trackbacks are not enabled");
            }

            IPost post = PostRepository.GetPost(id);

            if (post == null)
            {
                return new TrackbackErrorResult(0, "ID is invalid or missing");
            }

            if (Request.RequestType == "POST")
            {
                string incomingUrl = getParameter(ControllerContext.HttpContext, "url");
                string incomingTitle = getParameter(ControllerContext.HttpContext, "title");
                string incomingBlogName = getParameter(ControllerContext.HttpContext, "blog_name");
                string incomingExcerpt = getParameter(ControllerContext.HttpContext, "excerpt");

                if (incomingUrl == "")
                {
                    return new TrackbackErrorResult(1, "no url parameter found, please try harder!");
                }

                ITrackback trackback = TrackbackRepository.GetTrackback(post, incomingUrl);

                try
                {
                    if (trackback == null)
                    {
                        trackback = TrackbackRepository.CreateTrackback();

                        trackback.Post = post;
                        trackback.Title = incomingTitle;
                        trackback.Body = incomingExcerpt;
                        trackback.Url = incomingUrl;
                        trackback.BlogName = incomingBlogName;
                        trackback.Source = "";
                        trackback.Created = trackback.Modified = DateTime.Now.ToUniversalTime();

                        TrackbackRepository.AddTrackback(trackback);
                    }
                    else
                    {
                        trackback.Title = incomingTitle;
                        trackback.Body = incomingExcerpt;
                        trackback.BlogName = incomingBlogName;
                        trackback.IsTargetInSource = null;
                        trackback.Modified = DateTime.Now.ToUniversalTime();
                    }

                    TrackbackRepository.SubmitChanges();

                    return new TrackbackSuccessResult(post);
                }
                catch
                {
                    return new TrackbackErrorResult(2, "Failed to save Trackback.");
                }
            }
            else
            {
                return new TrackbackErrorResult(1, "no POST data found, please try harder!");
            }
        }

        private static string getParameter(HttpContextBase context, string parameterName)
        {
            if (context.Request.Form[parameterName] != null)
            {
                return HttpUtility.HtmlEncode(context.Request.Form[parameterName]);
            }

            return "";
        }
    }
}